using System.Web.Mvc;
using EPiServer.Web.Mvc;
using BaseSite.Models.Pages;
using BaseSite.Models.ViewModels;

namespace BaseSite.Controllers
{
    public class HomePageController : PageController<HomePage>
    {
        public ActionResult Index(HomePage currentPage)
        {
            
            var model = PageViewModel.Create(currentPage);
                        
            return View(model);
        }

        public string GetFirstPargraph(string input)
        {
            if (input.Contains("<p>") && input.Contains("</p>"))
            {
                var start = input.IndexOf("<p>");
                var end = input.IndexOf("</p>");

                var result = input.Substring(start, end+4 - start);

                return result;
            }
            else
            {
                return input;
            }
        }
       
}
}