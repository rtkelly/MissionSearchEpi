using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using MissionSearch;
using MissionSearch.Attributes;

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "News Page", GUID = "9cf4659f-7c3b-486c-9c69-b8714b01a255", Description = "")]
    public class NewsPage : SearchableBasePage
    {
        [SearchIndex("title")]
        [CultureSpecific]
        [Display(Name = "Page Title", Order = 1)]
        public virtual String PageTitle { get; set; }

        [SearchIndex("summary")]
        [CultureSpecific]
        [Display(Name = "Summary", Order = 2)]
        public virtual String Summary { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "Introduction", Order = 3)]
        public virtual XhtmlString Intro { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "Main Body", Order = 4)]
        public virtual XhtmlString MainBody { get; set; }

        [SearchIndex("timestamp")]
        [Display(Name = "Published Date", Order = 5)]
        public virtual DateTime PublishedDate { get; set; }

        [SearchIndex("featured")]
        [CultureSpecific]
        [Display(Name = "Featured", Order = 0)]
        public virtual bool Featured { get; set; }

          
      

        

    }
}