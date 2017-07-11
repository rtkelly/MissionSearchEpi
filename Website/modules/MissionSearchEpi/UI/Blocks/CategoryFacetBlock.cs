using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearch;
using MissionSearchEpi.UI.EditorDescriptors;
using System;
using System.ComponentModel.DataAnnotations;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Category Facet", GUID = "21426b2f-5fbe-4c05-8e5a-4ab910e0c53c", Description = "")]
    public class CategoryFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {
        [Ignore]
        public string FieldName { get; set; }

        [Required]
        [Display(Name = "Heading", Order = 10)]
        public virtual String Label { get; set; }

        [Required]
        [Display(Name = "Facet Category", Order = 20)]
        public virtual CategoryList CategoryName { get; set; }
                
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