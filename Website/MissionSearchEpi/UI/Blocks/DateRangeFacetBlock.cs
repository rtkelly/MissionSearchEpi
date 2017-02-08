using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.EditorDescriptors;
using MissionSearchEpi.UI.EditorDescriptors;
using MissionSearch.Attributes;


namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Date Range Facet", GroupName = "Search - Facet Field", GUID = "70d9688e-9b85-4ad0-b28b-273d64bda859", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class DateRangeFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {
        [Required]
        [Display(Name = "Field Name", Order = 1)]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchDocTaggedEditorDescriptor<SearchDocument, DateFacetField>))]
        public virtual String FieldName { get; set; }

        [Required]
        [Display(Name = "Facet Label", Order = 2)]
        public virtual String Label { get; set; }
                        
        [Display(Name = "Custom Range", Order = 3)]
        public virtual string RangeGap { get; set; }

        [Required]
        [Display(Name = "Range Years", Order = 4)]
        public virtual int MaxRange { get; set; }
                
    }
}