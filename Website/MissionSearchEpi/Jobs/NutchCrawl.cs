using System;
using EPiServer.PlugIn;
using EPiServer.Scheduler;

namespace MissionSearchEpi.Jobs
{
    [ScheduledPlugIn(DisplayName = "Search - Run Nutch External Crawler", DefaultEnabled = true, SortIndex = 5)]
    public class NutchCrawl : ScheduledJobBase
    {
        private bool _stopSignaled;

        public NutchCrawl()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            var dateStart = DateTime.Now;

            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(string.Format("Starting External Crawl at {0:hh mm ss tt}", dateStart));

            var crawler = MissionSearch.SearchFactory.Crawler;

            if(crawler != null)
                crawler.Crawl("crawl1", "default");

            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            var duration = DateTime.Now - dateStart;

            return string.Format("Crawl Finished. Duration ({0:00}:{1:00}:{2:00}) ", duration.Hours, duration.Minutes, duration.Seconds);   

        }
    }
}
