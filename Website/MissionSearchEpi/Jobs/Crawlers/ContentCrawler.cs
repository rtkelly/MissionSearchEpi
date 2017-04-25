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
using System.Reflection;

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
        /// <param name="crawlSettings"></param>
        public ContentCrawler(CrawlerSettings crawlSettings)
        {
            Repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();

            _contentIndexer = SearchFactory<T>.ContentIndexer;

            _crawlSettings = crawlSettings;

            _logger = SearchFactory.Logger;
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

            _logger = SearchFactory.Logger;
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
        private List<ContentCrawlParameters> GetSearchablePages(PageReference rootPage, DateTime? crawlStartDate)
        {
            var searchablePages = new List<ContentCrawlParameters>();

            if (rootPage == null)
                return searchablePages;

            if (_crawlSettings.ExcludedPages != null && _crawlSettings.ExcludedPages.Contains(rootPage.ID))
                return searchablePages;

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

                            foreach (var pageInstance in pageLanguages)
                            {
                                var clone = pageInstance.CreateWritableClone();

                                var searchablePage = BuildSearchablePage(clone);

                                if (searchablePage != null)
                                {
                                    searchablePages.Add(searchablePage);
                                }
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
        public ContentCrawlParameters BuildSearchablePage(PageData page)
        {
            var searchablePage = page as ISearchableContent;

            if (searchablePage == null)
                return null;

            searchablePage._ContentID = string.Format("{0}-{1}", page.ContentGuid, page.Language.Name);
       
            var pageCrawlParameters = new ContentCrawlParameters();
            
            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "title",
                Value = page.Name,
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "timestamp",
                Value = page.Changed,
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "url",
                Value = page.LinkURL.Replace("epslanguage=en", string.Format("epslanguage={0}", page.Language.Name)),
            });
            
            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "contentid",
                Value = page.ContentLink.ID.ToString(),
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "categories",
                Value = EpiHelper.GetCategoryPaths(page.Category),
            });
                        
            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "hostname",
                Value = EpiHelper.GetSitePath(page.ContentLink),
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "folder",
                Value = EpiHelper.GetParentName(page.ParentLink.ToPageReference()),
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "path",
                Value = EpiHelper.GetPageTreePath(page.ParentLink.ToPageReference()),
            });
            
            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "paths",
                Value = EpiHelper.GetPageTreePaths(page.ParentLink.ToPageReference()),
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "pagetype",
                Value = page.PageTypeName,
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "mimetype",
                Value = "text/html",
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "contenttype",
                Value = "HTML",
            });
            
            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "language",
                Value = new List<string>()
                {
                    page.Language.Name,
                },
            });

            // if enabled scrape page content 
            if (_crawlSettings.PageScrapper != null && pageCrawlParameters.EnablePageScrape)
            {
                var scrapContent = _crawlSettings.PageScrapper.ScrapPage(EpiHelper.GetExternalURL(page));

                pageCrawlParameters.Content.Add(new CrawlerContent()
                {
                    Name = MissionSearch.Global.ContentField,
                    Value = scrapContent,
                });
            }

            // parse searchable block data and add to content
            var parsedBlockText = ProcessContentReferences(page);

            if (parsedBlockText.Any())
            {
                pageCrawlParameters.Content.AddRange(parsedBlockText);
            }

            //searchablePage.CrawlProperties = pageCrawlMetadata;
            pageCrawlParameters.ContentItem = searchablePage;

            return pageCrawlParameters;
        }

        /// <summary>
        /// Method used to pull serachable content from ContentAreas and lists of ContentReferences
        /// </summary>
        /// <param name="contentData"></param>
        public List<CrawlerContent> ProcessContentReferences(ContentData contentData)
        {
            var list = new List<CrawlerContent>();

            if (contentData == null)
                return list;

            var pageProps = contentData.GetType().GetProperties();
                        

            foreach (var pageProp in pageProps)
            {
                try {
                    if (pageProp.Name == "ContentLink")
                        continue;

                    if (pageProp.Name == "Item")
                        continue;

                    //_logger.Debug(string.Format("Processing {0}", pageProp.Name));

                    var propValue = pageProp.GetValue(contentData);

                    if (propValue == null)
                        continue;

                    switch(pageProp.PropertyType.Name)
                    {
                        case "ContentArea":
                    
                            var refContentArea = propValue as ContentArea;

                            if (refContentArea == null)
                                continue;

                            foreach (var contentRef in refContentArea.Items)
                            {
                                var blockData = Repository.Get<IContent>(contentRef.ContentLink) as BlockData;
                                
                                if (blockData != null)
                                {
                                    var fields = ParseBlockData(blockData);

                                    if (fields.Any())
                                        list.AddRange(fields);
                                }
                            }

                            break;


                        case "IList`1":
                        
                            var refList = propValue as IList<ContentReference>;

                            if (refList == null)
                                continue;

                            foreach (var contentRef in refList)
                            {
                                var blockData = Repository.Get<IContent>(contentRef) as BlockData;

                                if (blockData != null)
                                {
                                    var fields = ParseBlockData(blockData);

                                    if (fields.Any())
                                        list.AddRange(fields);
                                }
                            }

                            break;

                        case "ContentReference":
                            
                                var blockRef = propValue as ContentReference;

                                var blockData1 = Repository.Get<IContent>(blockRef) as BlockData;

                                if (blockData1 != null)
                                {
                                    var fields = ParseBlockData(blockData1);

                                    if (fields.Any())
                                        list.AddRange(fields);
                                }

                                break;
                            

                        default:

                            var blockData2 = propValue as BlockData;

                            if (blockData2 != null)
                            {
                                var blockFields = ParseBlockData(blockData2);

                                if (blockFields.Any())
                                    list.AddRange(blockFields);
                            }
                                                    
                        
                            break;
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error(string.Format("{0} \"{1}\" {2}", ex.Message, pageProp.Name, ex.StackTrace));
                    
                }
            }


            return list;
        }
             

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockData"></param>
        /// <returns></returns>
        private List<CrawlerContent> ParseBlockData(BlockData blockData)
        {
            var list = new List<CrawlerContent>();

            if (blockData == null) return list;

            var blockProps = blockData.GetType().GetProperties();

            foreach (var blockProp in blockProps)
            {
                var customAttr = Attribute.GetCustomAttributes(blockProp, typeof(SearchIndex));

                if (customAttr.Any())
                {
                    var srchFieldMap = customAttr.First() as SearchIndex;

                    if (srchFieldMap == null)
                        continue;

                    var fieldName = srchFieldMap.FieldName ?? "content";

                    var value = blockProp.GetValue(blockData);

                    if (value == null)
                        continue;

                    list.Add(new CrawlerContent()
                    {
                        Name = fieldName,
                        Value = value,
                    });
                }
            }

            var more = ProcessContentReferences(blockData);

            if (more.Any())
            {
                list.AddRange(more);
            }

            return list;
        }

        

    }


}