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
        public string _ContentID { get; set; }

        [Display(Name = "Not Searchable", Order = 1, GroupName = Global.GroupNames.SearchSettings)]
        public virtual bool NotSearchable { get; set; }
        

        [SearchIndex("summary")]
        [Display(Name = "Teaser Text", Order = 1)]
        public virtual string TeaserText { get; set; }

             
    }
}