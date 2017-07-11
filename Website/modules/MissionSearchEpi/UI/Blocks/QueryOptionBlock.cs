using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using MissionSearchEpi.UI.EditorDescriptors;

namespace MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Advanced Query Option", GroupName = "Search - Query Option", GUID = "acc844a7-ed7a-47ff-a33b-fc0f4dcb224a", Description = "")]
    [EditorDescriptor(EditorDescriptorType = typeof(HideCategoryEditorDescriptor))]
    public class QueryOptionBlock : BlockData, ISearchBlock
    {
        /*
        
        public enum QueryOptionType
        {
            bq = 0,
            fq = 1,
          
        }

        public enum BoostOption
        {
            EqualTo = 0,
            NotEqualTo = 1,
        }

        [CultureSpecific]
        [Display(Name = "Query Parmater", Order = 0)]
        public virtual String ParmName { get; set; }
       
        [CultureSpecific]
        [Display(Name = "Query Option", Order = 0)]
        [BackingType(typeof(PropertyNumber))]
        [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<QueryOptionType>))]
        public virtual QueryOptionType QueryOption { get; set; }
        */


        [Display(Name = "Query Fields", Order = 100)]
        public virtual String qf { get; set; }

        [Display(Name = "Minimum Match", Order = 200)]
        public virtual String mm { get; set; }

        [Display(Name = "Phrase Field Boost", Order = 300)]
        public virtual String pf { get; set; }

        [Display(Name = "Phrase Slope", Order = 400)]
        public virtual String ps { get; set; }
                

        //[Display(Name = "Query Phrase Slope", Order = 200)]
        //public virtual int qs { get; set; }
       
        
        
    }
}