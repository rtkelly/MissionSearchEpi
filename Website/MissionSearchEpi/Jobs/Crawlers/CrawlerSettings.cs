using MissionSearch.Util;
using System.Collections.Generic;
using System.Globalization;

namespace MissionSearchEpi.Crawlers
{
    public class CrawlerSettings
    {
        public IEnumerable<int> ExcludedPages {get; set; }
        
        public PageScrapper PageScrapper { get; set; }

        public CultureInfo Language { get; set; }
    }


    public class CrawlerPageSettings
    {
        private Dictionary<string, object> CrawlSettings { get; set; }

        public CrawlerPageSettings(Dictionary<string, object>  settings)
        {
            CrawlSettings = settings;
        }


        public bool EnablePageScrape
        {
            get
            {
                if (CrawlSettings == null)
                    return false;

                if (CrawlSettings.ContainsKey("EnablePageScrape"))
                {
                    if (CrawlSettings["EnablePageScrape"] is bool)
                    {
                        var b = CrawlSettings["EnablePageScrape"] as bool?;

                        return b.Value;
                    }
                }
                return false;
            }
        }
    
    }
}