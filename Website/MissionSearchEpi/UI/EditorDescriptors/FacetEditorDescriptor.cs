using EPiServer.Core;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using MissionSearchEpi.UI.Blocks;

namespace MissionSearchEpi.UI.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "Facets")]
    public class FacetEditorDescriptor : EditorDescriptor
    {
        public FacetEditorDescriptor()
        {
            AllowedTypes = new[] { typeof(FieldFacetBlock), typeof(DateRangeFacetBlock), typeof(RangeFacetBlock) };

            ClientEditingClass = "epi-cms.contentediting.editors.ContentAreaEditor";
            OverlayConfiguration.Add("customType", "epi-cms.widget.overlay.ContentArea");
        }
    }
}