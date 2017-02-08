using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseSite.Models.Blocks;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using System;

namespace BaseSite.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "FooterMenu")]
    public class FooterMenuDescriptor : EditorDescriptor
    {
        public FooterMenuDescriptor()
        {
            AllowedTypes = new Type[] { typeof(FooterMenuBlock) };

            ClientEditingClass = "epi-cms.contentediting.editors.ContentAreaEditor";
            OverlayConfiguration.Add("customType", "epi-cms.widget.overlay.ContentArea");
        }
    }
}