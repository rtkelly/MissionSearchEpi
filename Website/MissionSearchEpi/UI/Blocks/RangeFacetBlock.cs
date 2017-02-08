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
    [ContentType(DisplayName = "Range Facet", GroupName = "Search - Facet Field", GUID = "c9bbb206-3e86-42eb-9375-7e1cee061382", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class RangeFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {
        [Required]
        [Display(Name = "Field Name", Order = 1)]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchDocTaggedEditorDescriptor<SearchDocument, MissionSearch.Attributes.RangeFacetField>))]
        public virtual String FieldName { get; set; }

        [Required]
        [Display(Name = "Facet Label", Order = 2)]
        public virtual String Label { get; set; }

        [Required]
        [Display(Name = "Start", Order = 3)]
        public virtual double Start { get; set; }

        [Required]
        [Display(Name = "Range Gap", Order = 4)]
        public virtual string RangeGap { get; set; }

        [Display(Name = "Numeric Format", Order = 5)]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<NumRangeFacet.FormatType>))]
        public virtual NumRangeFacet.FormatType NumericFormat { get; set; }

       

    }
}