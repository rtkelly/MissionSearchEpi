using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using MissionSearchEpi.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;
using MissionSearch;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Sort Option", GroupName = "Search - Sort Field", GUID = "ec3cc7d0-d903-4631-b6d6-17ecbf8321b4", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class SortBlock : BlockData, ISearchBlock
    {
        [Required]
        [CultureSpecific]
        [Display(Name = "Field Name", Order = 1)]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchDocTaggedEditorDescriptor<SearchDocument, MissionSearch.Attributes.SortField>))]
        public virtual String FieldName { get; set; }
        
        [Required]
        [CultureSpecific]
        [Display(Name = "Sort Order", Order = 3)]
        [BackingType(typeof(PropertyNumber))]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<SortOrder.SortOption>))]
        public virtual SortOrder.SortOption SortOrder { get; set; }
    }
}