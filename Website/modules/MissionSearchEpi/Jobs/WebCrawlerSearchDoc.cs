﻿using MissionSearch;
using MissionSearch.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionSearchEpi.Jobs
{
    public class WebCrawlerSearchDoc : ISearchDocument
    {
        public string id { get; set; }
                
        public string title { get; set; }

        public string summary { get; set; }

        public string url { get; set; }

        public string highlightsummary { get; set; }

        public List<string> content { get; set; }
                
        public DateTime timestamp { get; set; }

        public int sourceid { get; set; }

        public string hostname { get; set; }

        public string contenttype { get; set; }

        public DateTime publishdate { get; set; }

        public List<string> categories { get; set; }

        public string sitename { get; set; }
    }
}
