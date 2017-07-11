using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using MissionSearchEpi.UI.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;
using MissionSearch.Attributes;
using EPiServer.Web;
using MissionSearch;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Filter Query", GroupName = "Search - Query Option", GUID = "048b8388-32d2-41bc-9ca6-e7bd2200e74a", Description = "Filter Query ")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class FieldQueryBlock : BlockData, ISearchBlock
    {

        [Required]
        [CultureSpecific]
        [Display(Name = "Filter Field", Order = 10)]
        [EditorDescriptor(EditorDescriptorType = typeof(SelectionEditorDescriptor<SearchDocTaggedSelectionFactory<SearchDocument, FilterField>>))]
        public virtual String FieldName { get; set; }

        [Required]
        [Display(Name = "Filter Condition", Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<FilterQuery.ConditionalTypes>))]
        public virtual FilterQuery.ConditionalTypes Condition { get; set; }
               
        [Required]
        [CultureSpecific]
        [Display(Name = "Filter Value", Order = 30)]
        [UIHint(UIHint.Textarea)]
        public virtual String FieldValue { get; set; }

        
    }
}