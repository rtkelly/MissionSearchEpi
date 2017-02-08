using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using MissionSearchEpi.EditorDescriptors;
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
        [Display(Name = "Filter Field", Order = 1, Description="Select the search index field to filter on.")]
        //[EditorDescriptor(EditorDescriptorType = typeof(SearchDocTypedEditorDescriptor<SearchDocument, String>))]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchDocTaggedEditorDescriptor<SearchDocument, FilterField>))]
        public virtual String FieldName { get; set; }

        [Required]
        [Display(Name = "Filter Condition", Order = 2, Description=" ")]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<FilterQuery.ConditionalTypes>))]
        public virtual FilterQuery.ConditionalTypes Condition { get; set; }
               
        [Required]
        [CultureSpecific]
        [Display(Name = "Filter Value", Order = 4, Description="The value to filter on")]
        [UIHint(UIHint.Textarea)]
        public virtual String FieldValue { get; set; }

        
    }
}