﻿using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace MissionSearchEpi.EditorDescriptors
{
    public class EnumEditorDescriptor<TEnum> : EditorDescriptor
    {
        public override void ModifyMetadata(
            ExtendedMetadata metadata,
            IEnumerable<Attribute> attributes)
        {
            SelectionFactoryType = typeof(EnumSelectionFactory<TEnum>);

            ClientEditingClass =
                "epi.cms.contentediting.editors.SelectionEditor";

            base.ModifyMetadata(metadata, attributes);
        }
    }
}