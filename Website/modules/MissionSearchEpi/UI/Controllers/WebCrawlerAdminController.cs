using MissionSearch.Crawlers;
using MissionSearch.Util;
using MissionSearchEpi.Config;
using MissionSearchEpi.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissionSearchEpi.UI.Controllers
{
    [Authorize(Roles = "CmsAdmins")] 
    public class WebCrawlerAdminController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var viewModel = new WebCrawlerAdminViewModel();

            return View("~/modules/MissionSearchEpi/UI/Views/WebCrawlerAdmin/index.cshtml", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var webCrawlJob = new WebCrawlJobConfig();

            webCrawlJob.Depth = 3;
            webCrawlJob.CrawlUrlPattern = ".*";
            webCrawlJob.IndexUrlPattern = ".*";

            return View("~/modules/MissionSearchEpi/UI/Views/WebCrawlerAdmin/add.cshtml", webCrawlJob);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public ActionResult Update(int sourceId)
        {
            var job = WebCrawlerConfig.GetJobConfigData(sourceId);

            return View("~/modules/MissionSearchEpi/UI/Views/WebCrawlerAdmin/add.cshtml", job);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webCrawlJob"></param>
        /// <returns></returns>
        public ActionResult Save(WebCrawlJobConfig webCrawlJob)
        {
            WebCrawlerConfig.SaveConfigData(webCrawlJob);            

            return RedirectToAction("Index", "WebCrawlerAdmin");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int sourceid)
        {
            WebCrawlerConfig.DeleteConfigData(sourceid);
            
            return RedirectToAction("Index", "WebCrawlerAdmin");
        }
    }
}