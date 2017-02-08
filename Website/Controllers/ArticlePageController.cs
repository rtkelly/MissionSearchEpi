using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using BaseSite.Models.ViewModels;
using BaseSite.Models.Pages;

namespace BaseSite.Controllers
{
    public class ArticlePageController : PageController<ArticlePage>
    {
        public ActionResult Index(ArticlePage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            return View(model);
        }
    }
}