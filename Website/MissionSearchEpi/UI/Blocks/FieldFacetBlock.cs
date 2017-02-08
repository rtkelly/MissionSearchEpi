using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.EditorDescriptors;
using MissionSearchEpi.UI.EditorDescriptors;
using MissionSearch;


namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Facet", GroupName = "Search - Facet Field", GUID = "a912af06-12b2-4fc4-87b9-9b9743c020e5", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class FieldFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {
        [Required]
        [Display(Name = "Field Name", Order = 1)]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchDocTaggedEditorDescriptor<SearchDocument, MissionSearch.Attributes.FacetField>))]
        public virtual String FieldName { get; set; }

        [Required]
        [Display(Name = "Facet Label", Order = 2)]
        public virtual String Label { get; set; }

        [Required]
        [CultureSpecific]
        [Display(Name = "Sort Option", Order = 3)]
        [BackingType(typeof(PropertyNumber))]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<FacetSortOption>))]
        public virtual FacetSortOption SortOption { get; set; }


    }
}