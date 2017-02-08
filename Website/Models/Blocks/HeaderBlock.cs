using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer;
using EPiServer.Web;
using EPiServer.Shell.ObjectEditing;

namespace BaseSite.Models.Blocks
{
    [ContentType(DisplayName = "Header", GUID = "8d271fbf-d8ca-413a-a69e-791bd02326ab", Description = "")]
    public class HeaderBlock : BlockData
    {
        [Display(Name = "Site Logo", Order = 200)]
        [DefaultDragAndDropTarget]
        [UIHint(UIHint.Image)]
        public virtual Url LogoSrc { get; set; }

        [CultureSpecific]
        [Display(Name = "Logo Alt Text", Order = 300)]
        public virtual String LogoAltText { get; set; }

        [UIHint("MainMenu")]
        [Display(Name = "Main Menu", Order = 400)]
        public virtual ContentArea MainMenu { get; set; }
        
    }
}