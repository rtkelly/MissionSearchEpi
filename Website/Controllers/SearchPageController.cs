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
               //RefinementType = RefinementTypes.MultiSelect,
               PageSize = 10,
               CurrentPage = TypeParser.ParseInt(Request["page"], 1),
               //QueryIndexer = SearchContainer<QuerySuggesterDocument>.QuerySuggesterClient,
               //EnableQueryLogging = true,
            };
                        
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
            
            var terms = client.GetMatches(Request["term"], 1).Take(5);
                        

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