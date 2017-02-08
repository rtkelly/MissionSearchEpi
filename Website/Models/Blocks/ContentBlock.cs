 using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using MissionSearch.Attributes;
using EPiServer;
using MissionSearch;
using EPiServer.Web;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "ContentBlock", GUID = "62472a43-e45a-4927-bdb9-5bf9aa80d3ef", Description = "")]
    public class ContentBlock : BlockBase
    {
        [SearchIndex]
        [CultureSpecific]
        [Editable(true)]
        [Display(Name = "Title", GroupName = SystemTabNames.Content, Order = 100)]
        public virtual String Title { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "SubTitle", Description = "Title", GroupName = SystemTabNames.Content, Order = 150)]
        public virtual String SubTitle { get; set; }

        [SearchIndex]
        [CultureSpecific]
        [Display(Name = "Content", GroupName = SystemTabNames.Content, Order = 200)]
        public virtual XhtmlString Content { get; set; }

        [CultureSpecific]
        [Display(Name = "Link Text", Description = "Link Text", GroupName = SystemTabNames.Content, Order = 300)]
        public virtual String LinkText { get; set; }

        [CultureSpecific]
        [Display(Name = "Link Url", Description = "Link Url", GroupName = SystemTabNames.Content, Order = 400)]
        public virtual Url LinkUrl { get; set; }

        [CultureSpecific]
        [Display(Name = "Image", GroupName = SystemTabNames.Content, Order = 500)]
        [UIHint(UIHint.Image)] //Displays an image instead of a ContentReference
        public virtual ContentReference Image { get; set; }

        
    }
}