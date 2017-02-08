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
    [ContentType(DisplayName = "Standard Page", GUID = "572edaf4-d8b2-4be5-99c3-27ce1c76cd85", Description = "")]
    public class StandardPage : SearchableBasePage
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
        [Display(Name = "Main Body", Order = 3)]
        public virtual XhtmlString MainBody { get; set; }

        [SearchIndex("timestamp")]
        [Display(Name = "Published Date", Order = 4)]
        public virtual DateTime PublishedDate { get; set; }

        [SearchIndex("featured")]
        [CultureSpecific]
        [Display(Name = "Featured", Order = 0)]
        public virtual bool Featured { get; set; }
                
      
        

    }
}