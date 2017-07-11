using System.Collections.Generic;

namespace MissionSearchEpi.UI.ViewModels
{
    public class SearchAdminViewModel : SearchAdminViewModelBase
    {
        public string DefaultClient { get; set; }
        
        public string Index { get; set; }
        
        public string LastContentCrawlDate { get; set; }
        
        public string LastAssetCrawlDate { get; set; }

        public string SearchServer { get; set; }
        
        public string SearchClient { get; set; }
        
        public string Status { get; set; }
        
        public string CrawlerPageExclusions { get; set; }

        public int IndexTotalItems { get; set; }
        
        public Dictionary<string, long> IndexCounts { get; set; }

        public Dictionary<string, List<string>> Synonyms { get; set; }

        public bool IndexOnPublishContent { get; set; }
        
        public bool IndexOnPublishAsset { get; set; }

        
        
    }
}