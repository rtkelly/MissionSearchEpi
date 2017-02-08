using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using BaseSite.Models.Media;
using EPiServer;
using EPiServer.Web;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "Footer", GUID = "6ced1aef-a294-4a10-acf8-520fc90a5d55", Description = "")]
    public class FooterBlock : BlockData
    {
        [UIHint("FooterMenu")]
        [Display(Name = "Footer Menu", Order = 100)]
        public virtual ContentArea FooterMenu { get; set; }
        
        [Display(Name = "Footer Logo", Order=200)]
        [DefaultDragAndDropTarget]
        [UIHint(UIHint.Image)]
        public virtual Url LogoSrc { get; set; }

        [CultureSpecific]
        [Display(Name = "Logo Alt Text", Order=300)]
        public virtual String LogoAltText { get; set; }

        [CultureSpecific]
        [Display(Name = "Copyright", Order=400)]
        public virtual XhtmlString MainBody { get; set; }
    }
}