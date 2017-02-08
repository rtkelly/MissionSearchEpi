using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "Footer Menu", GUID = "b1080daa-2446-44b6-b50b-abd70fe79d62", Description = "")]
    public class FooterMenuBlock : BlockBase
    {
        [CultureSpecific]
        public virtual String Title { get; set; }

        [Display(Name = "Menu Items")]
        [BackingType(typeof(PropertyLinkCollection))]
        public virtual LinkItemCollection Links { get; set; }
    }
}