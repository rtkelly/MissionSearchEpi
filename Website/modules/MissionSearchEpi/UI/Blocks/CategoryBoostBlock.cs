using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.Blocks;
using MissionSearchEpi.UI.EditorDescriptors;

namespace BaseSite.modules.MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Category Boost Block", GroupName = "Search - Query Option", GUID = "646c1b4e-4f3c-41a5-888d-f14777238b30", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class CategoryBoostBlock : BlockData, ISearchBlock
    {
        [Required]
        [Display(Order = 1)]
        public virtual CategoryList Categories { get; set; }

        [Required]
        [Display(Name = "Boost Value", Order = 2)]
        public virtual double Boost { get; set; }
    }
}