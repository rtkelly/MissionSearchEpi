using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using MissionSearchEpi.UI.Blocks;
using System;
using System.Collections.Generic;

namespace MissionSearchEpi.UI.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(CategoryList))]
    public class HideCategoryEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(
           ExtendedMetadata metadata,
           IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            var contentMetadata = (ContentDataMetadata)metadata;

            var ownerContent = contentMetadata.OwnerContent;

            if (metadata.PropertyName == "icategorizable_category")
            {
                var blockData = ownerContent as BlockData;

                if (blockData is ISearchBlock)
                {
                    metadata.ShowForEdit = false;
                }

            }
        }
    }
}