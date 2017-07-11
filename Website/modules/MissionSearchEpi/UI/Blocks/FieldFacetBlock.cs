using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;
using MissionSearch;


namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Facet", GroupName = "Search - Facet Field", GUID = "a912af06-12b2-4fc4-87b9-9b9743c020e5", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class FieldFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {
        [Required]
        [Display(Name = "Heading", Order = 10)]
        public virtual String Label { get; set; }

        [Required]
        [Display(Name = "Facet Field", Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(SelectionEditorDescriptor<SearchDocTaggedSelectionFactory<SearchDocument, MissionSearch.Attributes.FacetField>>))]
        public virtual String FieldName { get; set; }
                
        [Required]
        [CultureSpecific]
        [Display(Name = "Sort By", Order = 30)]
        [BackingType(typeof(PropertyNumber))]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<FacetSortOption>))]
        public virtual FacetSortOption SortOption { get; set; }

        [Required]
        [CultureSpecific]
        [Display(Name = "Facet Type", Order = 40)]
        [BackingType(typeof(PropertyNumber))]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<RefinementType>))]
        public virtual RefinementType RefinementType { get; set; }

    }
}