using MissionSearch;
using MissionSearch.Clients;
using MissionSearch.Clients.Solr;
using MissionSearchEpi.UI.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissionSearchEpi.Controllers
{
    [Authorize(Roles = "CmsAdmins")] 
    public class SynonymsAdminController : Controller
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // TO DO: confirm that only admin users have access to search admin

            var view = new SynonymsAdminViewModel();

            try
            {
                var mgr = new SolrResources<SearchDocument>();

                var managedSynonyms = mgr.GetManagedSynonymResources();

                if (!managedSynonyms.Any())
                    return View("~/modules/MissionSearchEpi/UI/Views/SynonymsAdmin/notManaged.cshtml");

                view.Synonyms = mgr.GetSynonyms("english");

                return View("~/modules/MissionSearchEpi/UI/Views/SynonymsAdmin/index.cshtml", view);
            }
            catch
            {
                return View("~/modules/MissionSearchEpi/UI/Views/SynonymsAdmin/notManaged.cshtml", view);
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var synonymMap = new Synonym();

            //HttpContext.Current.Application["shellPath"] = EpiHelper.GetShellPath();

            return View("~/modules/MissionSearchEpi/UI/Views/SynonymsAdmin/add.cshtml", synonymMap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            var mgr = new SolrResources<SearchDocument>();

            var term = Request["term"];
                        
            var synonymViewData = mgr.GetSynonym("english", term);

            if (synonymViewData.SynonymList.Contains(term))
            {
                synonymViewData.Synonyms = string.Join(",", synonymViewData.SynonymList.Where(t => t != term));
            }

            return View("~/modules/MissionSearchEpi/UI/Views/SynonymsAdmin/add.cshtml", synonymViewData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {
            var mgr = new SolrResources<SearchDocument>();

            var term = Request["term"];

            mgr.GetSynonym("english", term);

            
            //foreach (var sterm in synonym.SynonymList)
            //{
                 //mgr.DeleteSynonym("english", HttpUtility.UrlEncode(sterm));
            //}
            
            mgr.DeleteSynonym("english", HttpUtility.UrlEncode(term));

            return RedirectToAction("Index", "SynonymsAdmin");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synonymAddData"></param>
        /// <returns></returns>
        public ActionResult Save(Synonym synonymAddData)
        {
            var term = synonymAddData.Term.Trim().ToLower();
            var synonyms = synonymAddData.Synonyms.Split(',').ToList();

            synonyms.ForEach(s => s.Trim().ToLower());

            if (synonyms.Any())
            {
                var mgr = new SolrResources<SearchDocument>();

                if(!synonyms.Contains(term))
                    synonyms.Add(term);

                var s = new Synonym()
                {
                    Term = synonymAddData.Term,
                    Synonyms = string.Join(",", synonyms),
                };

                mgr.DeleteSynonym("english", s.Term);
                mgr.AddSynonym("english", s);

                mgr.ReloadCore();
            }

            return RedirectToAction("Index", "SynonymsAdmin");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Reload()
        {
            var client = SearchFactory<SearchDocument>.SearchClient as SolrClient<SearchDocument>;

            var mgr = new SolrResources<SearchDocument>(client);

            mgr.ReloadCore();

            return RedirectToAction("Index");
        }
	}
}