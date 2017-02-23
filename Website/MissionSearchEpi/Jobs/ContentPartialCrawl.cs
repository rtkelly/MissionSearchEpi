using System;
using EPiServer.PlugIn;
using MissionSearchEpi.Jobs;
using MissionSearchEpi.Config;
using MissionSearchEpi;
using MissionSearchEpi.Crawlers;
using MissionSearch.Util;
using MissionSearch;

namespace BaseSite.MissionSearchEpi.Jobs
{
    [ScheduledPlugIn(DisplayName = "Search - Run Content Crawler (Partial)", DefaultEnabled = false, SortIndex = 2)]
    public class ContentPartialCrawl : MissionScheduledJobBase
    {
        public ContentPartialCrawl()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            StopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            if (SearchFactory<SearchDocument>.ContentIndexer == null)
            {
                return "Indexer not configured";
            }

            OnStatusChanged(string.Format("Starting Crawl at {0:hh mm ss tt}", DateTime.Now));

            var configData = MissionConfig.GetConfigData();
                        
            var contentCrawler = new ContentCrawler<SearchDocument>(new CrawlerSettings()
                {
                    PageScrapper = new PageScrapper(),
                    ExcludedPages = TypeParser.ParseCSVIntList(configData.CrawlerPageExclusions),
                });

            var results = contentCrawler.RunCrawler(StatusCallback, configData.LastContentCrawledDate);
                        
            MissionConfig.SetLastContentCrawlDate(configData, DateTime.Now, results.Duration);
            
            OnStatusChanged("Done");

            return string.Format("Crawl Finished. {0} pages crawled. Total Errors {4}. Duration ({1:00}:{2:00}:{3:00}) ", results.TotalCnt, results.Duration.Hours, results.Duration.Minutes, results.Duration.Seconds, results.ErrorCnt);   
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
