using MissionSearch;
using MissionSearch.Attributes;
using System;
using System.Collections.Generic;

namespace MissionSearchEpi
{
    public class SearchDocument : ICMSSearchDocument
    {
        public string id { get; set; }

        [SortField]
        [DisplayName("Page Title")]
        public string title { get; set; }
        
        public string summary { get; set; }
                
        public string url { get; set; }
        
        public List<string> content { get; set; }

        [FacetField]
        [FilterField]
        public string hostname { get; set; }

        [FilterField]
        public List<string> language { get; set; }

        [FacetField]
        public bool featured { get; set; }

        [FacetField]
        [FilterField]
        [DisplayName("Category")]
        public List<string> categories { get; set; }
                        
        [SortField]
        [DateFacetField]
        [DisplayName("Published Date")]
        public DateTime timestamp { get; set; }
       
        [FacetField]
        [DisplayName("Publication")]
        public string pubname { get; set; }

        [FacetField]
        [FilterField]
        [DisplayName("Author")]
        public string author { get; set; }
                
        [FacetField]
        [FilterField]
        [DisplayName("Page Type")]
        public string contenttype { get; set; }

        [FacetField]
        public string mimetype { get; set; }

        [SortField]
        [RangeFacetField]
        [DisplayName("Price")]
        public double price { get; set; }
                
        public string highlightsummary { get; set; }
                
        public int sourceid { get; set; }
               
        public List<string> paths { get; set; }
       
        [FilterField]
        [DisplayName("CMS Path")]
        public string path { get; set; }

        public string folder { get; set; }







        public string contentid { get; set; }
    }

}