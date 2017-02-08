 using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using BaseSite.Models.Blocks;
using MissionSearch;
using MissionSearch.Attributes;
 

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "Landing Page", GUID = "3125fe78-ad20-445b-a029-2e5975aa31f0", Description = "")]
    public class LandingPage : SearchableBasePage
    {
        [SearchIndex("title")]
        [CultureSpecific]
        [Display(Name = "Page Title", Order = 1)]
        public virtual String PageTitle { get; set; }
                
        [UIHint("Slider")]
        [Display(Name = "Slider", Order = 2)]
        public virtual ContentArea Slider { get; set; }

        [SearchIndex("summary")]
        [CultureSpecific]
        [Display(Name = "Summary", Order = 3)]
        public virtual String Summary { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "Main Body", Order = 4)]
        public virtual XhtmlString MainBody { get; set; }
                        
        [Display(Name = "Main Content", Order = 5)]
        public virtual ContentArea MainContent { get; set; }

        [SearchIndex("timestamp")]
        [Display(Name = "Published Date", Order = 6)]
        public virtual DateTime PublishedDate { get; set; }

        [SearchIndex("featured")]
        [CultureSpecific]
        [Display(Name = "Featured", Order = 0)]
        public virtual bool Featured { get; set; }
        
       
                

      
    }
}