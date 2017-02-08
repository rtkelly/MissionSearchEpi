using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer;
using EPiServer.Web;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "CalloutBlock", GUID = "46a4e198-6524-45a1-8454-ea03fb6a2ea4", Description = "")]
    public class CalloutBlock : BlockData
    {
        public virtual String Heading { get; set; }

        [UIHint(UIHint.LongString)]
        public virtual String Summary { get; set; }

        [Display(Name = "Button Link")]
        public virtual Url ButtonUrl { get; set; }

        [Display(Name = "Button Link Text")]
        public virtual String ButtonLinkText { get; set; }
    }
}