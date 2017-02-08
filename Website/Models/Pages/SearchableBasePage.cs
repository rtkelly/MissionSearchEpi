using EPiServer;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using MissionSearch;
using MissionSearch.Attributes;
using MissionSearchEpi.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseSite.Models
{
    public class SearchableBasePage : BasePage, ISearchableContent
    {
        [Ignore]
        public string SearchId { get; set; }

        [Display(Name = "Not Searchable", Order = 1, GroupName = Global.GroupNames.SearchSettings)]
        public virtual bool NotSearchable { get; set; }
                
               
        public object _CrawlProperties;

        [Ignore]
        public object CrawlProperties
        {
            get
            {
                if (_CrawlProperties == null)
                {
                    var props = new Dictionary<string, object>();
                    props.Add("EnablePageScrape", (EnablePageScrape && !EpiHelper.IsRestrictedContent(this)));
                    _CrawlProperties = props;
                }

                return _CrawlProperties; 
            }

            set
            {
                _CrawlProperties = value;
            }
            
        }

        public bool EnablePageScrape
        {
            get
            {
                return false;
            }
        }

        [SearchIndex("summary")]
        [Display(Name = "Teaser Text", Order = 1)]
        public virtual string TeaserText { get; set; }

               
        [Ignore]
        public string SearchUrl { get; set; }

        [Ignore]
        public List<string> Languages { get; set; }

        [Ignore]
        public string HostName { get; set; }

        [Ignore]
        public List<string> PageTree { get; set; }
    }
}