using EPiServer.PlugIn;
using MissionSearch;
using MissionSearch.Crawlers;
using MissionSearch.Util;
using MissionSearchEpi.Config;
using MissionSearchEpi.Jobs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MissionSearchEpi.Jobs
{
    [ScheduledPlugIn(DisplayName = "Search - Run Web Crawler", DefaultEnabled = false, SortIndex = 6)]
    public class WebCrawlerScheduledJob : MissionScheduledJobBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Execute()
        {
            var jobs = WebCrawlerConfig.GetConfigData();

            var results = new List<CrawlerResults>();

            foreach(var job in jobs)
            {
                if (job.RunIntervalType == WebCrawlJobConfig.ScheduleIntervalType.Inactive)
                    continue;

                bool runJob = false;

                if (job.LastRunDate == null)
                {
                    runJob = true;
                }
                else
                {
                    switch (job.RunIntervalType)
                    {
                        case WebCrawlJobConfig.ScheduleIntervalType.Day:
                            if (job.LastRunDate.Value.AddDays(job.RunInterval) <= DateTime.Now)
                                runJob = true;
                            break;

                        case WebCrawlJobConfig.ScheduleIntervalType.Week:
                            if (job.LastRunDate.Value.AddDays(job.RunInterval * 7) <= DateTime.Now)
                                runJob = true;
                            break;

                        case WebCrawlJobConfig.ScheduleIntervalType.Month:
                            if (job.LastRunDate.Value.AddMonths(job.RunInterval) <= DateTime.Now)
                                runJob = true;
                            break;

                    }
                }

                if(runJob)
                {
                    var result = RunCrawler(job);
                    results.Add(result);

                    job.LastRunDate = DateTime.Now;

                    WebCrawlerConfig.SaveConfigData(job);

                }
                
            }

            if(results.Any())
            {
                var str = new StringBuilder();

                foreach (var result in results)
                {
                    str.Append(string.Format("Web Crawl (Source ID: {0}) Finished. {1} pages crawled. Total Errors {2}. Duration ({3:00}:{4:00}:{5:00}).\n", result.SourceId, result.TotalCnt, result.ErrorCnt, result.Duration.Hours, result.Duration.Minutes, result.Duration.Seconds));
                }

                return str.ToString();
            }

            return "";
        }

        private CrawlerResults RunCrawler(WebCrawlJobConfig jobConfig)
        {
            var job = new WebCrawlJob();

            job.SourceId = jobConfig.SourceId;
            job.SeedUrl = jobConfig.SeedUrl;
            job.Depth = jobConfig.Depth;

            if (!string.IsNullOrEmpty(jobConfig.CrawlUrlPattern))
                job.CrawlUrlPattern = jobConfig.CrawlUrlPattern.Replace('\r', ' ').Split('\n').Where(p => p != string.Empty).ToList();
            
            if (!string.IsNullOrEmpty(jobConfig.CrawlSkipUrlPattern))
                job.CrawlSkipUrlPattern = jobConfig.CrawlSkipUrlPattern.Replace('\r', ' ').Split('\n').Where(p => p != string.Empty).ToList();
            
            if (!string.IsNullOrEmpty(jobConfig.IndexUrlPattern))
                job.IndexUrlPattern = jobConfig.IndexUrlPattern.Replace('\r', ' ').Split('\n').Where(p => p != string.Empty).ToList();
            
            if (!string.IsNullOrEmpty(jobConfig.IndexSkipUrlPattern))
                job.IndexSkipUrlPattern = jobConfig.IndexSkipUrlPattern.Replace('\r', ' ').Split('\n').Where(p => p != string.Empty).ToList();

            if (!string.IsNullOrEmpty(jobConfig.ContentPattern))
                job.ContentPattern = jobConfig.ContentPattern.Replace('\r', ' ').Split('\n').Where(p => p != string.Empty).ToList();

            if (!string.IsNullOrEmpty(jobConfig.MetadataPattern))
                job.MetadataPattern = jobConfig.MetadataPattern.Replace('\r', ' ').Split('\n').Where(p => p != string.Empty).ToList();
            
            job.TitlePattern = jobConfig.TitlePattern;
            job.SummaryPattern = jobConfig.SummaryPattern;
            job.LinkCleanupPattern = new List<string>()
                {
                    ";jsessionid=.*$",
                    "#.*$",
                    
                };

            var crawler = new WebCrawler<WebCrawlerPage, WebCrawlerSearchDoc>(job, StatusCallback);

            var results = crawler.Run();
            return results;
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