using System.Linq;
using System.Web.Mvc;
using MissionSearch;
using MissionSearchEpi.Config;
using MissionSearch.Clients.Solr;
using MissionSearch.Clients;
using MissionSearchEpi.UI.ViewModels;

namespace MissionSearchEpi.Controllers
{
    [Authorize(Roles = "CmsAdmins")] 
    [EPiServer.PlugIn.GuiPlugIn(Area = EPiServer.PlugIn.PlugInArea.AdminMenu,
        Url = "/SearchAdmin", 
        DisplayName = "Search Admin")]
    public class SearchAdminController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new SearchAdminViewModel();

            var configData = MissionConfig.GetConfigData();

            if(configData.LastContentCrawledDate != null)
                viewModel.LastContentCrawlDate = configData.LastContentCrawledDate.Value.ToString("MM-dd-yyyy hh:mm:ss");

            if (configData.LastAssetCrawledDate != null)
                viewModel.LastAssetCrawlDate = configData.LastAssetCrawledDate.Value.ToString("MM-dd-yyyy hh:mm:ss");

            viewModel.IndexOnPublishContent = configData.IndexOnPublishContent;
            viewModel.IndexOnPublishAsset = configData.IndexOnPublishAsset;
            viewModel.CrawlerPageExclusions = configData.CrawlerPageExclusions;

            viewModel.Status = "Online";

            try
            {
                var srchClient = SearchFactory<SearchDocument>.SearchClient;
                viewModel.SearchServer = srchClient.SrchConnStr;
                viewModel.SearchClient = srchClient.GetType().Name;

                var req = new SearchRequest()
                {
                    QueryText = "*:*"
                };

                req.Facets.Add(new FieldFacet("mimetype", ""));

                var result = srchClient.Search(req);

                viewModel.IndexTotalItems = result.TotalFound;

                if (result.Refinements.Any())
                {
                    viewModel.IndexCounts = result.Refinements[0].Items.ToDictionary(t => t.Refinement, t => t.Count);
                }

                
                
            }
            catch
            {
                viewModel.Status = "Offline";
            }

            return View("~/MissionSearchEpi/UI/Views/SearchAdmin/index.cshtml", viewModel);
        }

        public ActionResult SaveSettings()
        {
            var indexContent = Request["IndexOnPublishContent"];
            var indexAsset = Request["IndexOnPublishAsset"];

            var configData = MissionConfig.GetConfigData();

            configData.IndexOnPublishContent = (indexContent.Contains("true"));
            configData.IndexOnPublishAsset = (indexAsset.Contains("true"));
            configData.CrawlerPageExclusions = Request["CrawlerPageExclusions"];

            MissionConfig.SaveConfigData(configData);

            return RedirectToAction("Index");
        }

        public ActionResult Reload()
        {
            var client = SearchFactory<SearchDocument>.SearchClient as SolrClient<SearchDocument>;

            var mgr = new SolrResources<SearchDocument>(client);

            mgr.ReloadCore();

            return RedirectToAction("Index");
        }
    }
}