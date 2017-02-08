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
    public class StandardPageController : PageController<StandardPage>
    {
        public ActionResult Index(StandardPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            return View(model);
        }
    }
}