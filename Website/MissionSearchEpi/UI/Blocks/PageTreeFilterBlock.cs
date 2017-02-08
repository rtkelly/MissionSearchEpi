using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.EditorDescriptors;
using MissionSearchEpi.UI.EditorDescriptors;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Page Tree Filter", GUID = "76d150d3-3966-4d0e-b893-10cda3827e14", Description = "", GroupName = "Search - Query Option")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class PageTreeFilterBlock : BlockData, ISearchBlock
    {
        public String FieldName { get { return "paths"; } }

        [Display(Name = "Filter On CMS Path", Order = 100, Description = "")]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchFieldEditorDescriptor<CmsPathSelectionFactory>))]
        public virtual String FieldValue { get; set; }
                
        //[Display(Name = "Include Sub folders", Order = 200, Description = "")]
        //public virtual Boolean IncludeSubFolders { get; set; }
    }
}