using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace MissionSearchEpi.UI.EditorDescriptors
{
    public class SelectionEditorDescriptor<T> : EditorDescriptor where T : ISelectionFactory
    {
        public override void ModifyMetadata(ExtendedMetadata metadata,  IEnumerable<Attribute> attributes)
        {
            SelectionFactoryType = typeof(T);

            ClientEditingClass =
                 "epi.cms.contentediting.editors.SelectionEditor";

            
            base.ModifyMetadata(metadata, attributes);
        }
    }
}