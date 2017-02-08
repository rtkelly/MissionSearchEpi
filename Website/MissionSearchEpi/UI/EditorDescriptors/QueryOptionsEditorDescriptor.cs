using EPiServer.Core;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using MissionSearchEpi.UI.Blocks;

namespace MissionSearchEpi.UI.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "QueryOption")]
    public class QueryOptionEditorDescriptor : EditorDescriptor
    {
        public QueryOptionEditorDescriptor()
        {
            AllowedTypes = new[] { typeof(BoostQueryBlock), typeof(FieldQueryBlock), typeof(QueryOptionBlock) };

            ClientEditingClass = "epi-cms.contentediting.editors.ContentAreaEditor";
            OverlayConfiguration.Add("customType", "epi-cms.widget.overlay.ContentArea");
        }
    }
}