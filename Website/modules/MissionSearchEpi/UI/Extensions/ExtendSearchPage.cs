using EPiServer.Core;
using MissionSearch;
using MissionSearch.Search.Facets;
using MissionSearch.Search.Query;
using MissionSearchEpi.UI.Blocks;
using MissionSearchEpi.Util;
using System.Collections.Generic;
using System.Linq;

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

                    if (filterBlock.FieldName == "pagetype")
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

            var facetBlocks = EpiHelper.GetContentAreaContent<BlockData>(srchPage.Facets.Items);

            foreach (var facetBlock in facetBlocks)
            {
                if(facetBlock is FieldFacetBlock)
                {
                    var fieldFacet = facetBlock as FieldFacetBlock;

                    facets.Add(new FieldFacet(fieldFacet.FieldName, fieldFacet.Label)
                    {
                        Sort = fieldFacet.SortOption,
                        Order = facetBlocks.IndexOf(facetBlock),
                        RefinementOption = fieldFacet.RefinementType,
                    });
                }
                else if(facetBlock is CategoryFacetBlock)
                {
                    var categoryFacet = facetBlock as CategoryFacetBlock;

                    var list = categoryFacet.CategoryName;

                    if (list.Any())
                    {
                        var categoryName = EpiHelper.GetCategoryName(list.First());
                        
                        facets.Add(new CategoryFacet("categories", categoryName, categoryFacet.Label)
                        {
                            Sort = categoryFacet.SortOption,
                            Order = facetBlocks.IndexOf(facetBlock),
                            RefinementOption = categoryFacet.RefinementType,
                        });

                    }
                }
                else if(facetBlock is DateRangeFacetBlock)
                {
                    var dateFacetBlock = facetBlock as DateRangeFacetBlock;
                    var dateFacet = ProcessDateFacetBlock(dateFacetBlock, facetBlocks.IndexOf(facetBlock));
                    facets.Add(dateFacet);
                }
                else if (facetBlock is RangeFacetBlock)
                {
                    var rangeFacetBlock = facetBlock as RangeFacetBlock;
                    var rangeFacet = ProcessRangeFacetBlock(rangeFacetBlock, facetBlocks.IndexOf(facetBlock));
                    facets.Add(rangeFacet);
                }
                

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

            var suggestedResultBlocks = EpiHelper.GetContentAreaContent<SuggestedResultsBlock>(srchPage.SuggestedResults.Items);
            
            var guidList = new List<string>();

            var queryIntercept = queryText.ToLower();

            foreach (var suggestedBlock in suggestedResultBlocks)
            {
                if (string.IsNullOrEmpty(suggestedBlock.Terms))
                    continue;

                var suggestedTerms = suggestedBlock.Terms.ToLower().Split(',')
                            .Select(str => str.Trim());

                if (suggestedTerms.Contains(queryIntercept))
                {
                    var results = EpiHelper.GetContentAreaContent<IContent>(suggestedBlock.SuggestedResults.Items);
                    
                    foreach (var result in results)
                    {
                        var id = result.ContentLink.ID.ToString();

                        if(!guidList.Contains(id))
                            guidList.Add(id);
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
                    req.QueryText = string.Format("contentid:\"{0}\"", guid);

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