using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using MissionSearchEpi.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Search Filter", GUID = "bdf104a4-6143-4c61-a3b4-8b76f6689dd8", GroupName = "Search - Query Option", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class FilterBlock : BlockData
    {
        [Display(Name = "Filter By CMS Path", Order = 100, Description = "")]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchFieldEditorDescriptor<CmsPathSelectionFactory>))]
        public virtual String FilterByCMSPath { get; set; }

        [Display(Name = "Filter By Category", Order = 200, Description = "")]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchFieldEditorDescriptor<CategorySelectionFactory>))]
        public virtual String FilterByCategory { get; set; }

        [Display(Name = "Filter By Page Type", Order = 200, Description = "")]
        [EditorDescriptor(EditorDescriptorType = typeof(SearchFieldEditorDescriptor<PageTypeSelectionFactory>))]
        public virtual String FilterByType { get; set; }
        
    }
}