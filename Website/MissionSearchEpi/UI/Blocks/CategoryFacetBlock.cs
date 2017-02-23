using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearch;
using MissionSearchEpi.EditorDescriptors;
using MissionSearchEpi.UI.Blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MissionSearchEpi.UI.Blocks
{
    public class CategoryFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {
        [Ignore]
        public string FieldName { get; set; }

        [Required]
        [Display(Name = "Category", Order = 1)]
        public virtual CategoryList CategoryName { get; set; }

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