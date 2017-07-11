using MissionSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissionSearchEpi.UI.ViewModels
{
    public class SearchDocAdmin : ISearchDocument
    {
        public string id { get; set; }

        public int sourceid { get; set; }

        public string title { get; set; }

        public string url { get; set; }

        public List<string> content { get; set; }

        public DateTime timestamp { get; set; }

        public string summary { get; set; }

        public string highlightsummary { get; set; }
    }
}