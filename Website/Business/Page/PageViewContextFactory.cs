using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;
using BaseSite.Models.Pages;
using BaseSite.Models.ViewModels;

namespace BaseSite.Business
{
    public class PageViewContextFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        
        public PageViewContextFactory(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public virtual LayoutModel CreateLayoutModel(ContentReference currentContentLink, RequestContext requestContext)
        {
            var startPage = _contentLoader.Get<HomePage>(SiteDefinition.Current.StartPage);

            return new LayoutModel() { 
                Header = startPage.Header,
                Footer = startPage.Footer,

            };

        }
    }
}