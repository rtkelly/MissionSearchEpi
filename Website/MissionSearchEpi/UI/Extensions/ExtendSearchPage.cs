using EPiServer.Core;
using MissionSearch;
using MissionSearch.Search.Facets;
using MissionSearch.Search.Query;
using MissionSearch.Util;
using MissionSearchEpi.UI.Blocks;
using MissionSearchEpi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MissionSearchEpi.Extensions
{
    public static partial class ExtendSearchPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srchPage"></param>
        /// <returns></returns>
        public static List<IQueryOption> ReturnQueryOptions(this ISearchPage srchPage)
        {
            var extraParms = new List<IQueryOption>();


            if (srchPage.QueryOptions != null)
            {
                var boostParms = EpiHelper.GetContentAreaContent<BoostQueryBlock> (srchPage.QueryOptions.Items);

                foreach (var bst in boostParms)
                {
                    extraParms.Add(new BoostQuery(bst.FieldName, bst.FieldValue, bst.Boost));
                }

                var filterParms = EpiHelper.GetContentAreaContent<FieldQueryBlock>(srchPage.QueryOptions.Items);

                foreach (var filterBlock in filterParms)
                {
                    var fieldValue = filterBlock.FieldValue;

                    if (filterBlock.FieldName == "contenttype")
                    {
                        if (filterBlock.FieldValue.Contains((" OR ")))
                        {
                            var replaced = filterBlock.FieldValue.Replace(" OR ", "%OR%");
                            fieldValue = replaced.Replace(" ", "").Replace("%OR%", " OR ");
                        }
                        else
                            fieldValue = filterBlock.FieldValue.Replace(" ", "");
                    }

                    if(filterBlock.FieldName == "path" && filterBlock.FieldValue.Contains('\\'))
                    {
                        fieldValue = filterBlock.FieldValue.Replace('\\', '/');
                    }

                    if (filterBlock.FieldName == "path" && filterBlock.Condition == FilterQuery.ConditionalTypes.Contains)
                    {
                        extraParms.Add(new FilterQuery("paths", FilterQuery.ConditionalTypes.Equals, fieldValue));
                    }
                    else
                    {
                        extraParms.Add(new FilterQuery(filterBlock.FieldName, filterBlock.Condition, fieldValue));
                    }
                }

                //var queryOptions = srchPage.QueryOptions.FilteredContents.OfType<QueryOptionBlock>();
                var queryOptions = EpiHelper.GetContentAreaContent<QueryOptionBlock>(srchPage.QueryOptions.Items);

                foreach (var option in queryOptions)
                {
                    if(!string.IsNullOrEmpty(option.mm))
                        extraParms.Add(new DisMaxQueryParm("mm", option.mm));
                    
                    if (!string.IsNullOrEmpty(option.ps))
                        extraParms.Add(new DisMaxQueryParm("ps", option.ps));

                    if (!string.IsNullOrEmpty(option.pf))
                        extraParms.Add(new DisMaxQueryParm("pf", option.pf));

                    if (!string.IsNullOrEmpty(option.qf))
                        extraParms.Add(new DisMaxQueryParm("qf", option.qf));

                    //extraParms.Add(new QueryParm("qs", option.qs.ToString()));
                    
                }
               
            }


            return extraParms;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<IFacet> ReturnFacetSettings(this ISearchPage srchPage)
        {
            var facets = new List<IFacet>();

            if (srchPage.Facets == null)
                return facets;
            
            var fieldFacetBlocks = EpiHelper.GetContentAreaContent<FieldFacetBlock>(srchPage.Facets.Items);

            foreach (var fieldFacet in fieldFacetBlocks)
            {
                facets.Add(new FieldFacet(fieldFacet.FieldName, fieldFacet.Label)
                {
                    Sort = fieldFacet.SortOption,
                    Order = fieldFacetBlocks.IndexOf(fieldFacet),
                });
            }

            var categoryFacetBlocks = EpiHelper.GetContentAreaContent<CategoryFacetBlock>(srchPage.Facets.Items);

            foreach (var categoryFacet in categoryFacetBlocks)
            {
                var list = categoryFacet.CategoryName;

                var categories = EpiHelper.GetCategoryNames(list);

                foreach (var category in categories)
                {
                    facets.Add(new CategoryFacet("categories", category, category)
                    {
                        Sort = categoryFacet.SortOption,
                        Order = categoryFacetBlocks.IndexOf(categoryFacet),
                    });
                    
                }
            }
            

            var dateFacetBlocks = EpiHelper.GetContentAreaContent<DateRangeFacetBlock>(srchPage.Facets.Items);

            foreach (var dateFacetBlock in dateFacetBlocks)
            {
                var dateFacet = ProcessDateFacetBlock(dateFacetBlock, dateFacetBlocks.IndexOf(dateFacetBlock));
                facets.Add(dateFacet);
            }

            var rangeFacetBlocks = EpiHelper.GetContentAreaContent<RangeFacetBlock>(srchPage.Facets.Items);

            foreach (var rangeFacetBlock in rangeFacetBlocks)
            {
                var rangeFacet = ProcessRangeFacetBlock(rangeFacetBlock, rangeFacetBlocks.IndexOf(rangeFacetBlock));
                facets.Add(rangeFacet);
            }

            
            return facets;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srchPage"></param>
        /// <returns></returns>
        /// 
       
        public static List<SortOrder> GetSortSettings(this ISearchPage srchPage)
        {
            var sorts = new List<SortOrder>();

            if (srchPage.Sort == null)
                return sorts;

            //var sortSettings = srchPage.Sort.Items.OfType<SortBlock>();
            var sortSettings = EpiHelper.GetContentAreaContent<SortBlock>(srchPage.Sort.Items);

            foreach (var s in sortSettings)
            {
                sorts.Add(new SortOrder(s.FieldName, s.SortOrder));
            }

            return sorts;
        }
     

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SortOrder> ReturnCurrentSort(this ISearchPage srchPage, string currentSort)
        {
            var sorts = new List<SortOrder>();

            if (!string.IsNullOrEmpty(currentSort))
            {
                // TO DO 01: not loading correct sort order
                sorts.Add(new SortOrder(currentSort, SortOrder.SortOption.Descending));
                
                return sorts;
            }
            
            if (srchPage.Sort == null)
                return sorts;

            //var sortSettings = srchPage.Sort.FilteredContents.OfType<SortBlock>();
            var sortSettings = EpiHelper.GetContentAreaContent<SortBlock>(srchPage.Sort.Items);
            

            foreach (var s in sortSettings)
            {
                sorts.Add(new SortOrder(s.FieldName, s.SortOrder));
            }

            return sorts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srchPage"></param>
        /// <param name="queryText"></param>
        /// <returns></returns>
        public static List<T> ReturnSuggestedResults<T>(this ISearchPage srchPage, string queryText) where T : ISearchDocument 
        {
            var suggResults = new List<T>();

            if (srchPage.SuggestedResults == null)
                return suggResults;

            //var suggestedResultBlocks = srchPage.SuggestedResults.FilteredContents.OfType<SuggestedResultsBlock>();
            var suggestedResultBlocks = EpiHelper.GetContentAreaContent<SuggestedResultsBlock>(srchPage.SuggestedResults.Items);
            
            var terms = queryText.Split(' ');

            var guidList = new List<string>();

            foreach (var suggestedBlock in suggestedResultBlocks)
            {
                var suggestedTerms = suggestedBlock.Terms.Split(',');

                if (terms.Intersect(suggestedTerms).Any())
                {
                    //var results = suggestedBlock.SuggestedResults.FilteredContents.OfType<IContent>();
                    var results = EpiHelper.GetContentAreaContent<IContent>(suggestedBlock.SuggestedResults.Items);
                    
                    foreach (var result in results)
                    {
                        guidList.Add(result.ContentGuid.ToString());
                    }
                }
            }

            if (guidList.Any())
            {
                var srchClient = SearchFactory<T>.SearchClient;

                foreach (var guid in guidList)
                {
                    var req = new SearchRequest();

                    req.EnableHighlighting = true;
                    req.QueryText = string.Format("id:\"{0}\"", guid);

                    var resp = srchClient.Search(req);

                    if (resp.Results.Any())
                    {
                        suggResults.Add(resp.Results.First());
                    }
                }

            }


            return suggResults;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srchPage"></param>
        /// <returns></returns>
        public static int GetPageSize(this ISearchPage srchPage)
        {
            if (srchPage.PageSize <= 0)
                return 10;

            return srchPage.PageSize;
        }


    }
}