using System;
using EPiServer.PlugIn;
using MissionSearchEpi.Crawlers;
using MissionSearchEpi.Config;
using MissionSearch;

namespace MissionSearchEpi.Jobs
{
    
    //[ScheduledPlugInWithParameters(DisplayName = "Run Asset Crawler (Incremental)", Description = "",
    //    DefinitionsClass = "BaseSite.Business.ScheduledJobs.Adaptors.Definitions.DocIncCrawlParameterDefinition",
    //    DefinitionsAssembly = "EPiServerSiteBase")]
    [ScheduledPlugIn(DisplayName = "Search - Run Asset Crawler (Partial)", SortIndex = 4, DefaultEnabled=false)]
    public class AssetPartialCrawl : MissionScheduledJobBase
    {
        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            var indexer = SearchFactory<SearchDocument>.AssetIndexer;

            if (indexer == null)
            {
                return "Indexer not configured";
            }

            OnStatusChanged(string.Format("Starting Crawl at {0:hh mm ss tt}", DateTime.Now));

            var configData = MissionConfig.GetConfigData();

            var crawler = new AssetCrawler<SearchDocument>(indexer);

            var results = crawler.RunCrawler(StatusCallback, configData.LastAssetCrawledDate);

            MissionConfig.SetLastAssetCrawlDate(configData, DateTime.Now, results.Duration);

            return string.Format("Crawl Finished. {0} assets crawled. {4} Errors. {5} Warnings. Duration ({1:00}:{2:00}:{3:00})", results.TotalCnt, results.Duration.Hours, results.Duration.Minutes, results.Duration.Seconds, results.ErrorCnt, results.WarningCnt);   
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
