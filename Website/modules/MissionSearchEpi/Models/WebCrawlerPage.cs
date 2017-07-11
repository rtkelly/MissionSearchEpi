using MissionSearch.Attributes;
using MissionSearch.Crawlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissionSearchEpi
{
    public class WebCrawlerPage : IWebCrawlPage
    {
        public string _ContentID { get; set; }

        //[MapHtmlNode("//body//h1")]
        public string Name { get; set; }

        public string SearchUrl { get; set; }

        public DateTime Changed { get; set; }

        public object CrawlProperties { get; set; }

        public bool NotSearchable { get; set; }

        public List<string> Content { get; set; }

        //[MapAttribute("//meta[@name='twitter:description']", "content")]
        [SearchIndex("summary")]
        public string Summary { get; set; }

        [SearchIndex("hostname")]
        public string Hostname { get; set; }

        [SearchIndex("contenttype")]
        public string ContentType { get { return "External Content"; } }

        [SearchIndex("publishdate")]
        //[MapAttribute("//meta[@property='article:published_time']", "content")]
        public DateTime Publishdate { get; set; }

        [SearchIndex("categories")]
        //[MapAttribute("//meta[@property='article:tag']", "content")]
        public List<string> Categories { get; set; }

        [SearchIndex("sitename")]
        //[MapAttribute("//meta[@property='og:site_name']", "content")]
        public string SiteName { get; set; }
    }
}