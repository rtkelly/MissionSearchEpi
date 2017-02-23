using System;
using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Shell.ObjectEditing;
using MissionSearch;
using System.Collections.Generic;
using System.Linq;
using BaseSite.Business.EditorDescriptors;
using BaseSite.Models;
using MissionSearchEpi;
using System.Web;
using MissionSearchEpi.UI.Blocks;

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "Search Page", GUID = "6b1ee001-54c9-4bf6-825c-a33d765498d4", Description = "")]
    public class SearchPage : BasePage, ISearchPage
    {
                
        [Display(Name = "Header Text", Order = 100)]
        public virtual string ResultsHeaderText { get; set; }

        [Display(Name = "Page Size", Order = 200)]
        public virtual int PageSize { get; set; }

        [Display(Name = "Query Options", Order = 300)]
        [AllowedTypes(typeof(FieldQueryBlock), typeof(BoostQueryBlock), typeof(QueryOptionBlock))]
        public virtual ContentArea QueryOptions { get; set; }

        [Display(Name = "Facets", Order = 400)]
        [AllowedTypes(typeof(FieldFacetBlock), typeof(RangeFacetBlock), typeof(DateRangeFacetBlock), typeof(CategoryFacetBlock))]
        public virtual ContentArea Facets { get; set; }
                
        [Display(Name = "Suggested Results", Order = 500)]
        [AllowedTypes(typeof(SuggestedResultsBlock))]
        public virtual ContentArea SuggestedResults { get; set; }

        [Display(Name = "Default Sort Order", Order = 600)]
        [AllowedTypes(typeof(SortBlock))]
        public virtual ContentArea Sort { get; set; }
        
        [CultureSpecific]
        [Display(Name = "No Results Message", Order = 700)]
        public virtual XhtmlString NoResults { get; set; }

      
    }
}