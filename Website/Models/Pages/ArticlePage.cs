 using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using MissionSearch;
using MissionSearchEpi;
using MissionSearch.Attributes;
using BaseSite.Models.Blocks;
using MissionSearchEpi.Models.Attributes;
using System.Collections.Generic;

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "Article Page", GUID = "33b12d5d-13cd-4445-80d2-9442a976dedb", Description = "")]
    public class ArticlePage : SearchableBasePage
    {
        [SearchIndex("title")]
        [CultureSpecific]
        [Display(Name = "Page Title", Order = 1)]
        public virtual String PageTitle { get; set; }

        
        [CultureSpecific]
        [Display(Name = "Summary", Order = 2)]
        public virtual String Summary { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "Sub Title", Order = 3)]
        public virtual String SubTitle { get; set; }

        [SearchIndex("summary")]
        [CultureSpecific]
        [Display(Name = "Introduction", Order = 4)]
        public virtual XhtmlString Intro { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "Main Body", Order = 5)]
        public virtual XhtmlString MainBody { get; set; }

        public virtual DateTime PublishedDate { get; set; }

        [SearchIndex("pubname")]
        [CultureSpecific]
        [Display(Name = "Publication Name", Order = 7)]
        public virtual String PublicationName { get; set; }

        [SearchIndex("author")]
        [CultureSpecific]
        [Display(Name = "Author", Order = 8)]
        public virtual String Author { get; set; }

        [SearchIndex("featured")]
        [CultureSpecific]
        [Display(Name = "Featured", Order = 0)]
        public virtual bool Featured { get; set; }

        
        [Display(Name = "Content Reference Test", Order = 100)]
        [AllowedTypes(new[] { typeof(ContentBlock) })]
        public virtual ContentReference ContentBlock1 { get; set; }

        
        [Display(Name = "Content Area Test", Order = 100)]
        [AllowedTypes(new[] { typeof(ContentBlock) })]
        public virtual ContentArea ContentBlock2 { get; set; }

        
        [Display(Name = "Content Ref List Test", Order = 100)]
        public virtual IList<ContentReference> RelatedContent { get; set; }


        [Display(Name = "Content Block Test", Order = 100)]
        public virtual ContentBlock ContentBlock3 { get; set; }
       
        
    }
}