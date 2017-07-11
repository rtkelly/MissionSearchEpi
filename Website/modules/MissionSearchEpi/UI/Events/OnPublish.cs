using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using MissionSearch;
using MissionSearchEpi.Config;
using EPiServer.Logging;
using System;
using MissionSearchEpi.Util;
using MissionSearchEpi.Crawlers;
using MissionSearch.Indexers;
using EPiServer;
using System.Web.Hosting;


namespace MissionSearchEpi.Events
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class OnPublish : IInitializableModule
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.PublishedContent += contentEvents_PublishedContent;
            contentEvents.MovedContent += contentEvents_MovingContent;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void contentEvents_MovingContent(object sender, ContentEventArgs e)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                try
                {
                    if (e.RequiredAccess == EPiServer.Security.AccessLevel.Delete)
                    {
                        DeleteFromIndex(e);
                    }
                    else //if (e.RequiredAccess == EPiServer.Security.AccessLevel.Publish)
                    {
                        PublishToIndex(e);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void contentEvents_PublishedContent(object sender, ContentEventArgs e)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                try
                {
                    PublishToIndex(e);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void PublishToIndex(ContentEventArgs e)
        {
            var configData = MissionConfig.GetConfigData();

            if (configData.IndexOnPublishContent && e.Content.GetType().IsSubclassOf(typeof(BlockData)))
            {
                var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

                var pages = repository.GetReferencesToContent(e.Content.ContentLink, false);

                if (pages == null)
                    return;

                var contentCrawler = new ContentCrawler<SearchDocument>(new CrawlerSettings());
                
                foreach (var pageRef in pages)
                {
                    var page = repository.Get<IContent>(pageRef.OwnerID);

                    if (page != null)
                    {
                        var pageData = page as PageData;

                        if (pageData != null)
                        {
                            var searchableContent = contentCrawler.BuildSearchablePage(pageData);

                            if (searchableContent != null)
                                SearchFactory<SearchDocument>.ContentIndexer.Update(searchableContent);
                        }
                    }
                }
            }
            else if (configData.IndexOnPublishAsset && e.Content is ISearchableAsset)
            {
                var mediaData = e.Content as MediaData;

                var crawler = new AssetCrawler<SearchDocument>();

                var searchableContent = crawler.BuildSearchableAsset(mediaData);

                if (searchableContent != null)
                    SearchFactory<SearchDocument>.AssetIndexer.Update(searchableContent);
            }
            else if (configData.IndexOnPublishContent && e.Content is ISearchableContent)
            {
                var pageData = e.Content as PageData;

                var contentCrawler = new ContentCrawler<SearchDocument>(new CrawlerSettings());

                var searchableContent = contentCrawler.BuildSearchablePage(pageData);

                if (searchableContent == null)
                    return;

                // if moving item then use target link instead of parent link
                if (e.TargetLink != null && e.TargetLink.ID != 0)
                {
                    var pageCrawlMetadata = searchableContent as ContentCrawlProxy;

                    if (pageCrawlMetadata != null)
                    {
                        pageCrawlMetadata.Content.Add(new CrawlerContent()
                        {
                            Name = "parent",
                            Value = EpiHelper.GetParentName(e.TargetLink.ToPageReference()),
                        });

                        pageCrawlMetadata.Content.Add(new CrawlerContent()
                        {
                            Name = "path",
                            Value = EpiHelper.GetPageTreePath(e.TargetLink.ToPageReference()),
                        });

                        pageCrawlMetadata.Content.Add(new CrawlerContent()
                        {
                            Name = "paths",
                            Value = EpiHelper.GetPageTreePaths(e.TargetLink.ToPageReference()),
                        });

                        pageCrawlMetadata.Content.Add(new CrawlerContent()
                        {
                            Name = "folder",
                            Value = EpiHelper.GetParentName(e.TargetLink.ToPageReference()),
                        });
                    }

                }
                                
                SearchFactory<SearchDocument>.ContentIndexer.Update(searchableContent);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void DeleteFromIndex(ContentEventArgs e)
        {
            var configData = MissionConfig.GetConfigData();

            if (e.Content is ISearchableAsset && configData.IndexOnPublishAsset)
            {
                var content = e.Content as ISearchableAsset;

                var indexer = SearchFactory<SearchDocument>.AssetIndexer;
                indexer.Delete(content);

            }
            else if (e.Content is ISearchableContent && configData.IndexOnPublishContent)
            {
                var pageData = e.Content as PageData;
                var id = string.Format("{0}-{1}", pageData.ContentGuid, pageData.Language);
                SearchFactory<SearchDocument>.SearchClient.DeleteById(id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public void Preload(string[] parameters) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }


    }
}