using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;
using MissionSearch.Attributes;


namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Date Range Facet", GroupName = "Search - Facet Field", GUID = "70d9688e-9b85-4ad0-b28b-273d64bda859", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class DateRangeFacetBlock : BlockData, IFacetBlock, ISearchBlock
    {

        [Required]
        [Display(Name = "Heading", Order = 10)]
        public virtual String Label { get; set; }
        
        [Required]
        [Display(Name = "Facet Field", Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(SelectionEditorDescriptor<SearchDocTaggedSelectionFactory<SearchDocument, DateFacetField>>))]
        public virtual String FieldName { get; set; }
                                
        [Display(Name = "Custom Range", Order = 30)]
        public virtual string RangeGap { get; set; }

        [Required]
        [Display(Name = "Range Years", Order = 40)]
        public virtual int MaxRange { get; set; }
               
                
    }
}