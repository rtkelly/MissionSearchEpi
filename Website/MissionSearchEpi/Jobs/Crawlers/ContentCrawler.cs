using EPiServer;
using EPiServer.Core;
using MissionSearch;
using MissionSearch.Attributes;
using MissionSearch.Indexers;
using MissionSearch.Util;
using MissionSearchEpi.Models.Attributes;
using MissionSearchEpi.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MissionSearchEpi.Crawlers
{
    public class ContentCrawler<T> where T : ISearchDocument
    {
        private IContentIndexer<T> _contentIndexer;

        private CrawlerSettings _crawlSettings;

        private IContentRepository Repository;

        private ILogger _logger { get; set; }

        public delegate bool StatusCallBack(string msg = null);

        /// <summary>
        /// 
        /// </summary>
        public ContentCrawler(CrawlerSettings crawlSettings)
        {
            Repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();

            _contentIndexer = SearchFactory<T>.ContentIndexer;

            _crawlSettings = crawlSettings;

            _logger = SearchFactory<T>.Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexer"></param>
        /// <param name="crawlSettings"></param>
        public ContentCrawler(IContentIndexer<T> indexer, CrawlerSettings crawlSettings)
        {
            Repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();

            _contentIndexer = indexer;

            _crawlSettings = crawlSettings;

            _logger = SearchFactory<T>.Logger;
        }


        /// <summary>
        /// Method used to start the content crawl. This method retrieves all searchable content from the CMS
        /// and calls the assigned indexer which will post the content to the search server. The crawler runs in 
        /// two modes. These modes are triggered by the crawlstartdate.
        /// 
        /// Full Crawl: When the crawlstartdate is null all searchable content is retrieved from the CMS
        /// and posted to the search server. When performing a full crawl the indexer will compare what is 
        /// posted with what is currently in the index and will deleting everything in the index that is not in
        /// the current set of posted items. 
        /// 
        /// Partial Crawl: When a crawlstartdate is defined then only content published on or after 
        /// the crawl start date will be retrieved and posted to the search server. In addition 
        /// deleted and archived items will be deleted from the search index. 
        /// </summary>
        /// <returns></returns>
        public IndexResults RunCrawler(Global<T>.StatusCallBack statusCallback, DateTime? crawlStartDate)
        {
            var dateStart = DateTime.Now;

            var fullCrawl = (crawlStartDate == null);

            var searchablePages = GetSearchablePages(ContentReference.RootPage, crawlStartDate);

            var results = (fullCrawl) ?
                _contentIndexer.RunFullIndex(searchablePages, statusCallback, IndexerCallback) :
                _contentIndexer.RunUpdate(searchablePages, statusCallback, IndexerCallback);

            if (!fullCrawl)
            {
                results.DeleteCnt = _contentIndexer.Delete(GetTrashCanPages(ContentReference.RootPage, crawlStartDate));
                results.DeleteCnt += _contentIndexer.Delete(GetArchivedPages(ContentReference.RootPage, crawlStartDate));
            }

            results.Duration = (DateTime.Now - dateStart);

            return results;
        }



        /// <summary>
        /// returns a list of all searchable content from the cms
        /// </summary>
        /// <param name="rootPage"></param>
        /// <param name="crawlStartDate"></param>
        /// <returns></returns>
        private List<ISearchableContent> GetSearchablePages(PageReference rootPage, DateTime? crawlStartDate)
        {
            var searchablePages = new List<ISearchableContent>();

            if (rootPage == null)
                return searchablePages;

            if (_crawlSettings.ExcludedPages != null && _crawlSettings.ExcludedPages.Contains(rootPage.ID))
                return searchablePages;

            //var pages = Repository.GetChildren<PageData>(rootPage, _crawlSettings.Language);
            var pages = Repository.GetChildren<PageData>(rootPage);

            foreach (var page in pages)
            {
                // to do: replace with id match
                if (page.Name == "Recycle Bin")
                    continue;

                var pageReference = page.ContentLink.ToPageReference();

                // do not index archived content
                if (page.StopPublish > DateTime.Now)
                {
                    if (page is ISearchableContent)
                    {
                        if (crawlStartDate == null || page.Changed >= crawlStartDate)
                        {
                            var pageLanguages = DataFactory.Instance.GetLanguageBranches(pageReference);

                            //var pageInstance = pageLanguages.FirstOrDefault(p => p.Language.Name == _crawlSettings.Language.Name);

                            //if(pageInstance == null)
                            //  pageInstance = page;

                            foreach (var pageInstance in pageLanguages)
                            {
                                var clone = pageInstance.CreateWritableClone();

                                var searchablePage = BuildSearchablePage(clone);

                                searchablePages.Add(searchablePage);
                            }

                        }
                    }

                    var childPages = GetSearchablePages(pageReference, crawlStartDate);

                    if (childPages.Any())
                    {
                        searchablePages.AddRange(childPages);
                    }
                }

            }

            return searchablePages;
        }

        /// <summary>
        /// Method used to pull serachable content from ContentAreas and lists of ContentReferences
        /// </summary>
        /// <param name="pageData"></param>
        /// <param name="pagecrawlMetadata"></param>
        public List<CrawlerContent> ParseSearchableContentReferences(PageData pageData, CrawlerContentSettings pagecrawlMetadata)
        {
            var list = new List<CrawlerContent>();

            if (pageData == null)
                return list;

            var pageContentAreas = pageData.GetType().GetProperties().Where(p => p.PropertyType.Name == "ContentArea").ToList();

            foreach (var contentAreaProp in pageContentAreas)
            {
                var contentArea = contentAreaProp.GetValue(pageData);

                if (contentArea == null)
                    continue;

                var customAttr = Attribute.GetCustomAttributes(contentAreaProp, typeof(SearchContentReference));

                if (!customAttr.Any()) continue;

                var blockRef = contentArea as ContentArea;

                if (blockRef == null)
                    continue;

                foreach (var contentRef in blockRef.Items)
                {
                    var fields = ParseContentReference(contentRef.ContentLink);

                    if (fields.Any())
                        list.AddRange(fields);
                }
            }

            var pageContentRefList = pageData.GetType().GetProperties().Where(p => p.PropertyType.Name == "IList`1").ToList();

            foreach (var contentRefListProp in pageContentRefList)
            {
                var contentRefList = contentRefListProp.GetValue(pageData);

                if (contentRefList == null)
                    continue;

                var customAttr = Attribute.GetCustomAttributes(contentRefListProp, typeof(SearchContentReference));

                if (!customAttr.Any()) continue;

                var refList = contentRefList as IList<ContentReference>;

                if (refList == null)
                    continue;

                foreach (var contentRef in refList)
                {
                    var fields = ParseContentReference(contentRef);

                    if (fields.Any())
                        list.AddRange(fields);

                }
            }

            var contentReferenceProps = pageData.GetType().GetProperties().Where(p => p.PropertyType.Name == "ContentReference" && p.Name != "ContentLink").ToList();

            foreach (var block in contentReferenceProps)
            {
                var blockCustomAttr = Attribute.GetCustomAttributes(block, typeof(SearchContentReference));

                if (!blockCustomAttr.Any()) continue;

                var blockData = block.GetValue(pageData);

                if (blockData == null)
                    continue;

                var blockRef = blockData as ContentReference;

                var fields = ParseContentReference(blockRef);

                if (fields.Any())
                    list.AddRange(fields);
            }

            var pageBlockProps = pageData.GetType().GetProperties()
                .Where(p => p.PropertyType.BaseType != null)
                .Where(p => p.PropertyType.BaseType.Name == "BlockData").ToList();

            foreach (var block in pageBlockProps)
            {
                var blockCustomAttr = Attribute.GetCustomAttributes(block, typeof(SearchContentReference));

                if (blockCustomAttr.Any())
                {
                    var blockData = block.GetValue(pageData) as BlockData;

                    if (blockData == null)
                        continue;

                    var fields = ParseBlockData(blockData);

                    if (fields.Any())
                        list.AddRange(fields);


                }
            }

            return list;
        }

        /// <summary>
        /// Method used to pull searchable content from blocks assigned to content references. 
        /// </summary>
        /// <param name="blockRef"></param>
        /// <returns></returns>
        private List<CrawlerContent> ParseContentReference(ContentReference blockRef)
        {
            var strBuilder = new List<CrawlerContent>();

            if (blockRef == null || blockRef.ID == 0)
                return strBuilder;

            var blockData = Repository.Get<IContent>(blockRef) as BlockData;
            
            return ParseBlockData(blockData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockData"></param>
        /// <returns></returns>
        private List<CrawlerContent> ParseBlockData(BlockData blockData)
        {
            var strBuilder = new List<CrawlerContent>();

            if (blockData == null) return strBuilder;

            var blockProps = blockData.GetType().GetProperties();

            foreach (var blockProp in blockProps)
            {
                var customAttr = Attribute.GetCustomAttributes(blockProp, typeof(SearchIndex));

                if (!customAttr.Any())
                    continue;

                var srchFieldMap = customAttr.First() as SearchIndex;

                if (srchFieldMap == null)
                    continue;

                var fieldName = srchFieldMap.FieldName ?? "content";

                var value = blockProp.GetValue(blockData);

                if (value == null)
                    continue;

                strBuilder.Add(new CrawlerContent()
                {
                    Name = fieldName,
                    Value = value,
                });

            }

            return strBuilder;
        }


        /// <summary>
        /// returns a list of all deleted pages from the cms
        /// </summary>
        /// <param name="rootPage"></param>
        /// <param name="crawlStartDate"></param>
        /// <returns></returns>
        private List<ISearchableContent> GetTrashCanPages(PageReference rootPage, DateTime? crawlStartDate)
        {
            var deletedPages = new List<ISearchableContent>();

            if (rootPage == null)
                return deletedPages;

            var pages = Repository.GetChildren<PageData>(rootPage).ToList();

            foreach (var page in pages)
            {
                if (page is ISearchableContent && page.IsDeleted)
                {
                    if (crawlStartDate == null || page.Deleted >= crawlStartDate.Value)
                        deletedPages.Add(page as ISearchableContent);
                }

                var childPages = GetTrashCanPages(page.ContentLink.ToPageReference(), crawlStartDate);

                if (childPages.Any())
                {
                    deletedPages.AddRange(childPages);
                }
            }

            return deletedPages;



        }

        /// <summary>
        /// returns a list of all archived pages from the cms
        /// </summary>
        /// <param name="rootPage"></param>
        /// <param name="crawlStartDate"></param>
        /// <returns></returns>
        private List<ISearchableContent> GetArchivedPages(PageReference rootPage, DateTime? crawlStartDate)
        {
            var archivePages = new List<ISearchableContent>();

            if (rootPage == null)
                return archivePages;

            var pages = Repository.GetChildren<PageData>(rootPage).ToList();

            foreach (var page in pages)
            {
                if (page is ISearchableContent
                        && (page.StopPublish <= DateTime.Now && (crawlStartDate == null || page.StopPublish >= crawlStartDate.Value)))
                {
                    archivePages.Add(page as ISearchableContent);
                }

                var childPages = GetArchivedPages(page.ContentLink.ToPageReference(), crawlStartDate);

                if (childPages.Any())
                {
                    archivePages.AddRange(childPages);
                }
            }

            return archivePages;



        }


        /// <summary>
        /// Call back method for the indexer. Use to make modifications to the seach document 
        /// before it is posted to the search server.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private T IndexerCallback(T doc, ISearchableContent page)
        {

            return doc;
        }

        /// <summary>
        /// Used to intialize default properties
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ISearchableContent BuildSearchablePage(PageData page)
        {
            var searchablePage = page as ISearchableContent;

            if (searchablePage == null)
                return null;

            var languages = new List<string>();
            languages.Add(page.Language.Name);

            searchablePage.SearchId = string.Format("{0}-{1}", page.ContentGuid, page.Language.Name);
            searchablePage.SearchUrl = page.LinkURL.Replace("epslanguage=en", string.Format("epslanguage={0}", page.Language.Name));

            var pageCrawlMetadata = new CrawlerContentSettings(searchablePage.CrawlProperties as Dictionary<string, object>);
            pageCrawlMetadata.Content = new List<CrawlerContent>();

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "contentid",
                Value = page.ContentLink.ID.ToString(),
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "categories",
                Value = EpiHelper.GetCategories(page.Category),
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "hostname",
                Value = EpiHelper.GetSitePath(page.ContentLink),
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "folder",
                Value = EpiHelper.GetParentName(page.ParentLink.ToPageReference()),
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "path",
                Value = EpiHelper.GetPageTreePath(page.ParentLink.ToPageReference()),
            });


            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "paths",
                Value = EpiHelper.GetPageTreePaths(page.ParentLink.ToPageReference()),
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "contenttype",
                Value = page.PageTypeName,
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "mimetype",
                Value = "text/html",
            });

            pageCrawlMetadata.Content.Add(new CrawlerContent()
            {
                Name = "language",
                Value = languages,
            });

            // if enabled scrape page content 
            if (_crawlSettings.PageScrapper != null && pageCrawlMetadata.EnablePageScrape)
            {
                var scrapContent = _crawlSettings.PageScrapper.ScrapPage(EpiHelper.GetExternalURL(page));

                pageCrawlMetadata.Content.Add(new CrawlerContent()
                {
                    Name = MissionSearch.Global.ContentField,
                    Value = scrapContent,
                });
            }

            // parse searchable block data and add to content
            var parsedBlockText = ParseSearchableContentReferences(page, pageCrawlMetadata);

            if (parsedBlockText.Any())
            {
                pageCrawlMetadata.Content.AddRange(parsedBlockText);
            }

            searchablePage.CrawlProperties = pageCrawlMetadata;

            return searchablePage;
        }

    }


}