using System;
using EPiServer.PlugIn;
using MissionSearchEpi.Crawlers;
using MissionSearchEpi.Config;
using MissionSearch.Util;
using MissionSearch;


namespace MissionSearchEpi.Jobs
{
    [ScheduledPlugIn(DisplayName = "Search - Run Content Crawler (Full)", DefaultEnabled = true, SortIndex = 1)]
    public class ContentFullCrawl : MissionScheduledJobBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            var indexer = SearchFactory<SearchDocument>.ContentIndexer;

            if (indexer == null)
            {
                return "Indexer not configured";
            }

            var configData = MissionConfig.GetConfigData();

            OnStatusChanged(string.Format("Starting Crawl at {0:hh mm ss tt}", DateTime.Now));

            try
            {
                var contentCrawler = new ContentCrawler<SearchDocument>(indexer, new CrawlerSettings()
                {
                    PageScrapper = new PageScrapper(),
                    ExcludedPages = TypeParser.ParseCSVIntList(configData.CrawlerPageExclusions),
                });

                var results = contentCrawler.RunCrawler(StatusCallback, null);

                MissionConfig.SetLastContentCrawlDate(configData, DateTime.Now, results.Duration);

                OnStatusChanged("Done");

                return string.Format("Crawl Finished. {0} pages crawled. Total Errors {4}. Duration ({1:00}:{2:00}:{3:00}) ", results.TotalCnt, results.Duration.Hours, results.Duration.Minutes, results.Duration.Seconds, results.ErrorCnt);
            }
            catch (Exception ex)
            {

                OnStatusChanged("failed");

                return string.Format("{0} {1}", ex.Message, ex.StackTrace);
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool StatusCallback(string msg)
        {
            if (StopSignaled)
            {
                OnStatusChanged("Job Stopped");
                return false;
            }

            return true;
        }








    }
}