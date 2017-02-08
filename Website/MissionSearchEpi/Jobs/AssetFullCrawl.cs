using System;
using MissionSearchEpi.Crawlers;
using EPiServer.PlugIn;
using MissionSearchEpi.Config;
using MissionSearch;

namespace MissionSearchEpi.Jobs
{
    [ScheduledPlugIn(DisplayName = "Search - Run Asset Crawler (Full)", DefaultEnabled = true, SortIndex = 3)]
    public class AssetFullCrawl : MissionScheduledJobBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            if (SearchFactory<SearchDocument>.AssetIndexer == null)
            {
                return "Indexer not configured";
            }

            OnStatusChanged(string.Format("Starting Crawl at {0:hh mm ss tt}", DateTime.Now));

            try
            {
                var crawler = new AssetCrawler<SearchDocument>();

                var configData = MissionConfig.GetConfigData();

                var results = crawler.RunCrawler(StatusCallback);

                MissionConfig.SetLastAssetCrawlDate(configData, DateTime.Now, results.Duration);

                return string.Format("Crawl Finished. {0} assets crawled. {4} Errors. {5} Warnings. Duration ({1:00}:{2:00}:{3:00})", results.TotalCnt, results.Duration.Hours, results.Duration.Minutes, results.Duration.Seconds, results.ErrorCnt, results.WarningCnt);
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