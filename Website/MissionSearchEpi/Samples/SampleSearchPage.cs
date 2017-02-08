using EPiServer.Core;
using EPiServer.DataAnnotations;
using MissionSearchEpi;
using MissionSearchEpi.UI.Blocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MissionSearchEpi.Samples
{
    public class SampleSearchPage : ISearchPage
    {
        [Display(Name = "Header Text", Order = 100)]
        public virtual string ResultsHeaderText { get; set; }

        [Display(Name = "Page Size", Order = 200)]
        public virtual int PageSize { get; set; }

        [Display(Name = "Query Options", Order = 300)]
        [AllowedTypes(typeof(FieldQueryBlock), typeof(BoostQueryBlock))]
        public virtual ContentArea QueryOptions { get; set; }

        [Display(Name = "Facets", Order = 400)]
        [AllowedTypes(typeof(FieldFacetBlock), typeof(RangeFacetBlock), typeof(DateRangeFacetBlock))]
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