using MissionSearchEpi.UI.ViewModels;
using MissionSearch;
using MissionSearch.Suggester;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MissionSearch.Util;

namespace MissionSearchEpi.UI.Controllers
{
    [Authorize(Roles = "CmsAdmins")] 
    public class SuggestedQueriesAdminController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var view = new SuggestedQueriesAdminViewModel();

            try
            {
                var client = SearchFactory<QuerySuggesterDocument>.QuerySuggesterClient;

                view.IndexTotal = client.GetIndexTotal();

                var currentPage = 1;

                var response = client.TermSearch(null, currentPage);

                //view.PagingInfo = response.BuildPagination("/SuggestedQueriesAdmin/Search?queryTerm=&page=");
                view.PagingInfo = response.PagingInfo;
                view.Terms = response.Results.Select(t => t.title).ToList();
                view.SearchServer = client.GetConnectionString();
                view.SearchClient = client.GetClientType();
                view.TotalFound = response.TotalFound;
                view.QueryTerm = "";

                return View("~/modules/MissionSearchEpi/UI/Views/SuggestedQueriesAdmin/index.cshtml", view);
            }
            catch
            {
                return View("~/modules/MissionSearchEpi/UI/Views/SuggestedQueriesAdmin/notManaged.cshtml", view);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var view = new SuggestedQueriesAdminViewModel();

            return View("~/modules/MissionSearchEpi/UI/Views/SuggestedQueriesAdmin/add.cshtml", view);

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="querySuggestions"></param>
        /// <returns></returns>
        public ActionResult Save(string querySuggestions)
        {
            if (!string.IsNullOrEmpty(querySuggestions))
            {
                var client = SearchFactory<QuerySuggesterDocument>.QuerySuggesterClient;

                var terms = querySuggestions.Split(',').ToList();
                                
                foreach(var term in terms)
                {
                    client.AddTerm(term, "en");
                }

                client.CommitTerms();
                client.Close();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryTerm"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Search(string queryTerm, int? page)
        {
            var view = new SuggestedQueriesAdminViewModel();

            var client = SearchFactory<QuerySuggesterDocument>.QuerySuggesterClient;

            view.IndexTotal = client.GetIndexTotal();

            var currentPage = TypeParser.ParseInt(Request["page"], 1);

            var response = client.TermSearch(queryTerm, currentPage);
                        
            //view.PagingInfo = response.BuildPagination("/SuggestedQueriesAdmin/Search?queryTerm=" + queryTerm);
            view.PagingInfo = response.PagingInfo;
            view.Terms = response.Results.Select(t => t.title).ToList();
            view.SearchServer = client.GetConnectionString();
            view.SearchClient = client.GetClientType();
            view.TotalFound = response.TotalFound;
            view.QueryTerm = queryTerm;

            return View("~/modules/MissionSearchEpi/UI/Views/SuggestedQueriesAdmin/index.cshtml", view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            var term = Request["queryterm"];

            var client = SearchFactory<QuerySuggesterDocument>.QuerySuggesterClient;

            client.RemoveTerm(term, "en");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
        {
            var client = SearchFactory<QuerySuggesterDocument>.QuerySuggesterClient;
                                   
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var lines = new List<string>();

                    var httpPostedFileBase = Request.Files[0];

                    if (httpPostedFileBase != null)
                    {
                        using (var textReader = new StreamReader(httpPostedFileBase.InputStream))
                        {
                            string line;

                            while ((line = textReader.ReadLine()) != null)
                            {
                                lines.Add(line);

                            }
                        }

                        int cnt = 0;

                        foreach (var line in lines)
                        {
                            client.AddTerm(line, "en");
                            cnt++;

                            if (cnt == 1000)
                            {
                                client.CommitTerms();
                                cnt = 0;
                                System.Threading.Thread.Sleep(5000);

                            }

                        }
                    }
                    client.CommitTerms();
                    client.Close();
                }
            }
             

            return RedirectToAction("Index");
        }

	}
}