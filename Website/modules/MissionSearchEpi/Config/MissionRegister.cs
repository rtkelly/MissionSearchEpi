using System.Web.Mvc;
using System.Web.Routing;

namespace MissionSearchEpi.Config
{
    public static class MissionRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public static void RegisterAll()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("SearchAdmin", "SearchAdmin/{action}", new { controller = "SearchAdmin", action = "Index" });
            routes.MapRoute("SynonymsAdmin", "SynonymsAdmin/{action}", new { controller = "SynonymsAdmin", action = "Index" });
            routes.MapRoute("SuggestedQueriesAdmin", "SuggestedQueriesAdmin/{action}", new { controller = "SuggestedQueriesAdmin", action = "Index" });
            routes.MapRoute("WebCrawlerAdmin", "WebCrawlerAdmin/{action}", new { controller = "WebCrawlerAdmin", action = "Index" });
            //routes.MapRoute("SearchPageAdmin", "SearchPageAdmin/{action}", new { controller = "SearchPageAdmin", action = "Index" });
            
            
            routes.MapMvcAttributeRoutes();
        
        } 

    }
}