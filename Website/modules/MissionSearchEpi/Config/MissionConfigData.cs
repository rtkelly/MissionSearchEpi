using System;

namespace MissionSearchEpi.Config
{
    public class MissionConfigData
    {
        public DateTime? LastContentCrawledDate { get; set; }
        
        public TimeSpan? LastContentCrawlDuration { get; set; }
        
        public long TotalContentCrawled { get; set; }

        public DateTime? LastAssetCrawledDate { get; set; }
        
        public TimeSpan? LastAssetCrawlDuration { get; set; }
        
        public long TotalAssetsCrawled { get; set; }

        public bool IndexOnPublishContent { get; set; }
        
        public bool IndexOnPublishAsset { get; set; }

        public string CrawlerPageExclusions { get; set; }
    }
}