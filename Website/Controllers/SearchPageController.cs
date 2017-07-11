using System.Linq;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using BaseSite.Models.Pages;
using BaseSite.Models.ViewModels;
using MissionSearch;
using MissionSearchEpi;
using MissionSearchEpi.Extensions;
using MissionSearch.Suggester;
using MissionSearch.Search.Query;
using MissionSearch.Util;
using System;
using System.Collections.Generic;

namespace BaseSite.Controllers
{
    public class SearchPageController : PageController<SearchPage>
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public ActionResult Index(SearchPage currentPage)
        {
            var view = new SearchViewModel<SearchResults>(currentPage);

            if (string.IsNullOrEmpty(Request["q"]))
                return View(view);

            var queryText = Request["q"];
            var sort = Request["sort"];
                        
            var request = new SearchRequest()
            {
               QueryText = queryText,
               QueryOptions = currentPage.ReturnQueryOptions(),
               Facets = currentPage.ReturnFacetSettings(),
               Sort = currentPage.ReturnCurrentSort(sort),
               Refinements = Request["ref"],
               EnableHighlighting = true,
               //RefinementType = RefinementType.Single_Select,
               PageSize = 10,
               CurrentPage = TypeParser.ParseInt(Request["page"], 1),
               //QueryIndexer = SearchContainer<QuerySuggesterDocument>.QuerySuggesterClient,
               //EnableQueryLogging = true,
            };

            //request.Sort = new System.Collections.Generic.List<SortOrder>() {
            //    new SortOrder("title_sortable", SortOrder.SortOption.Descending),
            //};

            //request.QueryOptions.Add(new FilterQuery("title", "duck"));
            //request.QueryOptions.Add(new FilterQuery("title", FilterQuery.ConditionalTypes.Contains, "du"));

            //request.Facets.Add(new CategoryFacet("categories", "Animal", "By Animal", RefinementType.MultiSelect));
            
            request.Facets.Add(new CategoryFacet("categories", "Organ System", "Organ System", RefinementType.Multi_Select));
            request.Facets.Add(new CategoryFacet("categories", "Topic", "Topic", RefinementType.Refinement));
            request.Facets.Add(new CategoryFacet("categories", "Content Type", "Content Type", RefinementType.Refinement));
            request.Facets.Add(new FieldFacet("pagetype", "Page Type", RefinementType.Refinement));

            //request.Facets.Add(new FieldFacet("contenttype", "Content Type"));

            /*
            request.Facets.Add(new CategoryFacet("categories", "Animal", "By Pet", RefinementType.MultiSelect));
            
            
            
            //request.Facets.Add(new CategoryFacet("categories", "Topic", "By Topic", RefinementType.Refinement));


            var dateFacet = new DateRangeFacet("timestamp", "Date", RefinementType.MultiSelect);
            request.Facets.Add(dateFacet);

            var seedDate = new DateTime(DateTime.Today.Year, 1, 1);
            
            dateFacet.Ranges.Add(new DateRange(seedDate, seedDate.AddYears(1), seedDate.Year));
            dateFacet.Ranges.Add(new DateRange(seedDate.AddYears(-1), seedDate, seedDate.AddYears(-1).Year));
            dateFacet.Ranges.Add(new DateRange(seedDate.AddYears(-2), seedDate.AddYears(-1), seedDate.AddYears(-2).Year));
            dateFacet.Ranges.Add(new DateRange(null, seedDate.AddYears(-2), seedDate.AddYears(-3).Year));
             * */

            //request.QueryOptions.Add(new BoostQuery("featured", "true", 1));
            //request.QueryOptions.Add(new BoostQuery("categories", "Press Release", 1));

            var response = SearchFactory<SearchResults>.SearchClient.Search(request);
            
            if (Request["debug"] != null)
                view.DebugQuery = true;
                        
            view.QueryText = queryText;
            view.Response = response;
            view.CurrentSort = sort;
            view.SuggestedResults = currentPage.ReturnSuggestedResults<SearchResults>(queryText);
            
            return View(view);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public ActionResult AutoComplete(SearchPage currentPage)
        {
            var client = SearchFactory<QuerySuggesterDocument>.QuerySuggesterClient;
            
            var terms = (client != null) ? client.GetMatches(Request["term"], 1).Take(5) : new List<string>();

            return Json(terms, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public ActionResult AutoComplete2(SearchPage currentPage)
        {
            var srchClient = SearchFactory<SearchDocument>.SearchClient;

            var terms = srchClient.GetTerms("_text_", Request["term"]);

            terms = terms.Where((elem, idx) => idx % 2 == 0).ToList();
            
            return Json(terms, JsonRequestBehavior.AllowGet);

        }
       
        

    }
}