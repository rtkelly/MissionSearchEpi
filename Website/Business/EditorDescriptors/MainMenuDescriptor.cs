using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseSite.Models.Blocks;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Core;

namespace BaseSite.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "MainMenu")]
    public class MainMenuDescriptor : EditorDescriptor
    {
        public MainMenuDescriptor()
        {
            AllowedTypes = new Type[] { typeof(MainMenuBlock) };

            ClientEditingClass = "epi-cms.contentediting.editors.ContentAreaEditor";
            OverlayConfiguration.Add("customType", "epi-cms.widget.overlay.ContentArea");
        }
    }
}