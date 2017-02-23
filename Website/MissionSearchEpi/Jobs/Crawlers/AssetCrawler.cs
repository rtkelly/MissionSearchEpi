using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using MissionSearch;
using MissionSearch.Indexers;
using System;
using System.Collections.Generic;
using System.Linq;
using MissionSearchEpi.Util;
using EPiServer.Web.Routing;
using EPiServer.DataAbstraction;
using MissionSearch.Util;

namespace MissionSearchEpi.Crawlers
{
    public class AssetCrawler<T> where T : ISearchDocument
    {
        private IContentRepository _repository;
        private IAssetIndexer<T> _assetIndexer;

        private List<string> _languages;

        private ILogger _logger { get; set; }

        private List<string> Languages
        {
            get
            {
                if (_languages == null)
                {
                    var langBranches = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>().ListEnabled();

                    _languages = langBranches.Select(l => l.Culture.Name).ToList();
                }

                return _languages;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AssetCrawler()
        {
            _repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            _assetIndexer = SearchFactory<T>.AssetIndexer;

            _logger = SearchFactory.Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexer"></param>
        public AssetCrawler(IAssetIndexer<T> indexer)
        {
            _repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            _assetIndexer = indexer;

            _logger = SearchFactory.Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCallback"></param>
        /// <param name="crawlStartDate"></param>
        /// <returns></returns>
        public IndexResults RunCrawler(Global<T>.StatusCallBack statusCallback, DateTime? crawlStartDate = null)
        {
            var dateStart = DateTime.Now;

            var rootFolder = _repository.Get<ContentFolder>(SiteDefinition.Current.GlobalAssetsRoot);

            var assets = CrawlFolders(new[] { rootFolder }, crawlStartDate);

            var fullCrawl = (crawlStartDate == null);

            var results = (fullCrawl) ?
                _assetIndexer.RunFullIndex(assets, statusCallback, IndexerCallback) :
                _assetIndexer.RunUpdate(assets, statusCallback, IndexerCallback);

            results.Duration = (DateTime.Now - dateStart);

            return results;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootFolders"></param>
        /// <param name="crawlStartDate"></param>
        /// <returns></returns>
        public List<ContentCrawlParameters> CrawlFolders(IEnumerable<ContentFolder> rootFolders, DateTime? crawlStartDate)
        {
            var assets = new List<ContentCrawlParameters>();

            foreach (var folder in rootFolders)
            {
                if (folder.Name == "Recycle Bin")
                    continue;

                var folderAssets = GetAssets(folder.ContentLink, crawlStartDate);

                if (folderAssets.Any())
                {
                    assets.AddRange(folderAssets);
                }

                var childFolders = _repository.GetChildren<ContentFolder>(folder.ContentLink).ToList();

                if (childFolders.Any())
                {
                    var childAssets = CrawlFolders(childFolders, crawlStartDate);

                    if (childAssets.Any())
                    {
                        assets.AddRange(childAssets);
                    }

                }
            }


            return assets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        private T IndexerCallback(T doc, ISearchableContent asset)
        {
            return doc;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="crawlStartDate"></param>
        /// <returns></returns>
        private List<ContentCrawlParameters> GetAssets(ContentReference folder, DateTime? crawlStartDate)
        {
            var assets = new List<ContentCrawlParameters>();

            var mediaData = _repository.GetChildren<MediaData>(folder);

            foreach (var mediaItem in mediaData)
            {

                if (mediaItem is ISearchableAsset)
                {
                    var searchableAsset = BuildSearchableAsset(mediaItem);

                    assets.Add(searchableAsset);
                }

            }

            return assets
                .Where(d => crawlStartDate == null || ((PageData)d.ContentItem).Changed >= crawlStartDate.Value).ToList();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaItem"></param>
        /// <returns></returns>
        public ContentCrawlParameters BuildSearchableAsset(MediaData mediaItem)
        {
            var searchableAsset = mediaItem as ISearchableAsset;

            if (searchableAsset == null)
                return null;

            var url = UrlResolver.Current.GetUrl(mediaItem.ContentLink);

            if (url == null) return null;

            searchableAsset._ContentID = string.Format("{0}", mediaItem.ContentGuid);

            try
            {
                searchableAsset.AssetBlob = EpiHelper.ReadEpiBlob(mediaItem.BinaryData);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.Error(string.Format("Asset Crawler: Error Retrieving Asset {0} {1}", ex.Message, ex.StackTrace));
            }
            
            var pageCrawlParameters = new ContentCrawlParameters();
            

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "title",
                Value = mediaItem.Name,
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "url",
                Value = url.Replace("?epslanguage=en", string.Format("?epslanguage={0}", mediaItem.Language.Name)),
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "pagetype",
                Value = "Media",
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "mimetype",
                Value = mediaItem.MimeType,
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "contenttype",
                Value = MimeType.GetDisplayName(mediaItem.MimeType),
            });
            
            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "language",
                Value = Languages,
            });

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "folder",
                Value = EpiHelper.GetParentFolderName(mediaItem.ParentLink.ToPageReference()),
            });


            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "paths",
                Value = EpiHelper.GetPageTreePaths(mediaItem.ParentLink.ToPageReference()),
            });
               

            pageCrawlParameters.Content.Add(new CrawlerContent()
            {
                Name = "path",
                Value = EpiHelper.GetFolderPath(mediaItem.ParentLink.ToPageReference()),
            });

            //searchableAsset.CrawlProperties = pageCrawlMetadata;
            pageCrawlParameters.ContentItem = searchableAsset;
            return pageCrawlParameters;
        }




    }
}