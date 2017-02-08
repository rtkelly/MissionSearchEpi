using EPiServer.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseSite
{
    public class Global
    {
        [GroupDefinitions()]
        public static class GroupNames
        {
            [Display(Name = "Site Settings", Order = 900)]
            public const string SiteSettings = "BaseSiteSettings";

            [Display(Name = "Search Settings", Order = 1000)]
            public const string SearchSettings = "SearchSiteSettings";
        }

        /// <summary>
        /// Names used for UIHint attributes to map specific rendering controls to page properties
        /// </summary>
        public static class SiteUIHints
        {
            public const string Contact = "contact";
            public const string Strings = "StringList";
        }
    }
}