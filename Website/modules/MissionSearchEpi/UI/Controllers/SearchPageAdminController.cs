using MissionSearch;
using MissionSearch.Util;
using MissionSearchEpi.UI.ViewModels;
using MissionSearchEpi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissionSearchEpi.UI.Controllers
{
    [RoutePrefix("SearchPageAdmin")]
    [Route("{action=index}")]
    [Authorize(Roles = "CmsAdmins")] 
    public class SearchPageAdminController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var viewModel = new SearchPageAdminViewModel<SearchDocAdmin>();

            return View("~/MissionSearchEpi/UI/Views/SearchPageAdmin/index.cshtml", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Route("SearchPageAdmin/Search")]
        public ActionResult Search()
        {
            var viewModel = new SearchPageAdminViewModel<SearchDocAdmin>();

            var client = SearchFactory<SearchDocAdmin>.SearchClient;

            var req = new SearchRequest()
            {
                QueryText = Request["q"],
                CurrentPage = TypeParser.ParseInt(Request["p"], 1),
                Refinements = Request["r"],
                PageSize = 10,
                EnableHighlighting = true,
            };

            foreach (var category in EpiHelper.GetRootCategories())
            {
                req.Facets.Add(new CategoryFacet("categories", category.Name, category.Name));
            }

            viewModel.Response = client.Search(req);
            viewModel.QueryText = Request["q"];

            return View("~/MissionSearchEpi/UI/Views/SearchPageAdmin/index.cshtml", viewModel);
        }

        
	}
}