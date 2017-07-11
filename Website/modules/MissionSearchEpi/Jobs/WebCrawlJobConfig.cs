using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissionSearchEpi.Config
{
    public class WebCrawlJobConfig
    {
        public int SourceId { get; set; }
        public string SeedUrl { get; set; }
        public string Description { get; set; }
        public int Depth { get; set; }
        public string CrawlUrlPattern { get; set; }
        public string CrawlSkipUrlPattern { get; set; }
        public string IndexUrlPattern { get; set; }
        public string IndexSkipUrlPattern { get; set; }
        public string LinkCleanupPattern { get; set; }
        public string TitlePattern { get; set; }
        public string SummaryPattern { get; set; }
        public string ContentPattern { get; set; }
        public string MetadataPattern { get; set; }
                
        //public bool IsActive { get; set; }
        public ScheduleIntervalType RunIntervalType { get; set; }
        public int RunInterval { get; set; }
        
        public DateTime? LastRunDate { get; set; }

        public enum ScheduleIntervalType
        {
            Inactive,
            Day,
            Week,
            Month
        }

    }
}