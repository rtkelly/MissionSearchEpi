using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.IO;
using EPiServer.Web.Routing;
using System.Linq;
using BaseSite.Business.Util;
using MissionSearch;
using MissionSearchEpi.Util;
using MissionSearch.Attributes;
using EPiServer;
using System.Collections.Generic;
using System.Web;

namespace BaseSite.Models.Media
{
    [ContentType(DisplayName = "DocumentFile", GUID = "4ffc025a-77aa-43cb-bb34-9f59ca3d35cf", Description = "")]
    [MediaDescriptor(ExtensionString = "pdf,doc,docx,mp3,xsl,xslx")]
    public class DocumentFile : MediaData, ISearchableAsset
    {
        [CultureSpecific]
        [Editable(true)]
        [Display(Name = "Description", GroupName = SystemTabNames.Content, Order = 1)]
        [SearchIndex("summary")]
        public virtual String Description { get; set; }

        [CultureSpecific]
        [Editable(true)]
        [SearchIndex("timestamp")]
        [Display(Name = "Publication Date", GroupName = SystemTabNames.Content,Order = 2)]
        public virtual DateTime PubDate { get; set; }
                
        [Display(Name = "Not Searchable", Order = 1, GroupName = Global.GroupNames.SearchSettings)]
        public virtual bool NotSearchable { get; set; }

        [Display(Name = "Disable Content Extraction", Order = 2, GroupName = Global.GroupNames.SearchSettings)]
        public virtual bool DisableExtract { get; set; }

        [Ignore]
        public string ContentID { get; set; }
      
        [Ignore]
        public object CrawlProperties { get; set; }
              
        [Ignore]
        public byte[] AssetBlob { get; set; }
                
        [Ignore]
        [SearchIndex("categories")]
        public List<string> Categories
        {
            get
            {
                return EpiHelper.GetCategoryNames(Category);
            }
        }

        [Ignore]
        public string _ContentID { get; set; }
    }
}