using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Suggested Results", GroupName = "Search - Suggested Results", GUID = "62839539-4c48-46f9-88af-5849088ecd45", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class SuggestedResultsBlock : BlockData, ISearchBlock
    {
        [Required]
        [CultureSpecific]
        [Display(Name = "CSV List of Terms", Order = 1, Description="Comma-seperated list of terms")]
        public virtual String Terms { get; set; }

        [Required]
        [Display(Name = "Suggested Results", Order = 2)]
        public virtual ContentArea SuggestedResults { get; set; }
    }
}