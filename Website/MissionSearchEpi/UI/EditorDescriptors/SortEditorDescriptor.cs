using EPiServer.Core;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using MissionSearchEpi.UI.Blocks;
using System;

namespace MissionSearchEpi.UI.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "SortOption")]
    public class SortOptionEditorDescriptor : EditorDescriptor
    {
        public SortOptionEditorDescriptor()
        {
            AllowedTypes = new[] { typeof(SortBlock) };

            ClientEditingClass = "epi-cms.contentediting.editors.ContentAreaEditor";
            OverlayConfiguration.Add("customType", "epi-cms.widget.overlay.ContentArea");
        }
    }
}