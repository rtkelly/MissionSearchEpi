using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace MissionSearchEpi.EditorDescriptors
{
    public class SearchDocTypedEditorDescriptor<DOC, T> : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            SelectionFactoryType = typeof(SearchDocTypedSelectionFactory<DOC, T>);

            ClientEditingClass =
                "epi.cms.contentediting.editors.SelectionEditor";

            base.ModifyMetadata(metadata, attributes);
        }
    }

}