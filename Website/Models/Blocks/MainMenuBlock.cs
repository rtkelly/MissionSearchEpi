using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "MainMenuBlock", GUID = "d1916ce4-1f9c-4442-805a-22eae71ea917", Description = "")]
    public class MainMenuBlock : BlockData
    {
        [CultureSpecific]
        public virtual String Title { get; set; }

        public virtual Url Link { get; set; }

        [Display(Name = "Menu Items")]
        [BackingType(typeof(PropertyLinkCollection))]
        public virtual LinkItemCollection Links { get; set; }

        [Ignore]
        public bool IsActive { get; set; }


        public MainMenuBlock IntializeMenu(BasePage page)
        {
            if (Link != null)
            {
                var currLink = page.LinkURL;
                var menuLink = Link.ToString();

                if (currLink.Contains(menuLink))
                {
                    IsActive = true;
                }
            }

            return this;
        }
    }
}