using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using BaseSite.Models.Pages;
using BaseSite.Models.ViewModels;

namespace BaseSite.Controllers
{
    public class LandingPageController : PageController<LandingPage>
    {
        public ActionResult Index(LandingPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            return View(model);
        }
    }
}