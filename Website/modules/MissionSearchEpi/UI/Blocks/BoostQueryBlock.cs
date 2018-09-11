using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Boost Query", GroupName = "Search - Query Option", GUID = "480315c4-e2d8-4cd8-86fc-dc2a131e680a", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class BoostQueryBlock : BlockData, ISearchBlock
    {
        [Required]
        [CultureSpecific]
        [Display(Name = "Boost on Field", Order = 1)]
        [EditorDescriptor(EditorDescriptorType = typeof(SelectionEditorDescriptor<SearchDocSelectionFactory<SearchDocument>>))]
        public virtual String FieldName { get; set; }

        [Required]
        [CultureSpecific]
        [Display(Name = "Matching Value", Order = 3)]
        public virtual String FieldValue { get; set; }

        [Required]
        [Display(Name = "Boost Value", Order = 4)]
        public virtual double Boost { get; set; }
    }
}