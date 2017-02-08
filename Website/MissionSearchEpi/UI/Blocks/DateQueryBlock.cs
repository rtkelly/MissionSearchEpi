using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearch.Search.Query;
using MissionSearchEpi;
using MissionSearchEpi.EditorDescriptors;
using MissionSearchEpi.UI.EditorDescriptors;

namespace MissionSearchEpi.UI.Blocks
{
    /*
    [ContentType(DisplayName = "Date Filter Query", GroupName = "Search - Query Option", GUID = "36a86159-afaa-4c70-ba48-27fcf5de58e1", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class DateQueryBlock : BlockData, ISearchBlock
    {
         
        [Required]
        [CultureSpecific]
        [Display(Name = "Field Name", Order = 1)]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchDocTypedEditorDescriptor<SearchDocument, DateTime>))]
        public virtual String FieldName { get; set; }

        [Required]
        [Display(Name = "Condition", Order = 2)]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<DateFilterQuery.ConditionalTypes>))]
        public virtual DateFilterQuery.ConditionalTypes Condition { get; set; }
               
        [Required]
        [CultureSpecific]
        [Display(Name = "Date", Order = 4)]
        public virtual DateTime FieldValue { get; set; }

    }
     * */
}