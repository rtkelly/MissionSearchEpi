using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using BaseSite.Models.Blocks;
using EPiServer;
using System.Collections.Generic;

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "Home Page", GUID = "ff9ed75b-8c7a-45d3-a485-c9027eca0368", Description = "")]
    public class HomePage : BasePage
    {
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 101)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(Name = "Jumbotron", Order=100)]
        public virtual JumbotronBlock JumboTron { get; set; }

        [Display(Name = "Main Content", Order = 200)]
        public virtual ContentArea MainContent { get; set; }
        

        [Display(Name = "Header", Order=300, GroupName = Global.GroupNames.SiteSettings)]
        public virtual HeaderBlock Header { get; set; }

        [Display(Name = "Footer", Order=400, GroupName = Global.GroupNames.SiteSettings)]
        public virtual FooterBlock Footer { get; set; }
                        
    }
}