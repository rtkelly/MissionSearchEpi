using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using MissionSearch;

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "Blog Post", GUID = "1d03ac96-1eca-4bab-9202-f443ecbe18bf", Description = "")]
    public class BlogPost : BasePage
    {
        [CultureSpecific]
        [Display(Name = "Page Title", Order = 1)]
        public virtual String PageTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Summary", Order = 2)]
        public virtual String Summary { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Main Body", Order = 3)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(Name = "Published Date", Order = 5)]
        public virtual DateTime PublishedDate { get; set; }

        [CultureSpecific]
        [Display(Name = "Featured", Order = 0)]
        public virtual bool Featured { get; set; }
      
       
       
    }
}