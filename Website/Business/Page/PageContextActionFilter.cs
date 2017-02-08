using System.Web.Mvc;
using EPiServer.Web.Routing;
using BaseSite.Models;
using BaseSite.Models.ViewModels;

namespace BaseSite.Business
{
    public class PageContextActionFilter : IResultFilter
    {
        private readonly PageViewContextFactory _contextFactory;
        
        public PageContextActionFilter(PageViewContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {

            var viewModel = filterContext.Controller.ViewData.Model;
            
            var model = viewModel as IPageViewModel<BasePage>;
            
            if (model != null)
            {
                
                var currentContentLink = filterContext.RequestContext.GetContentLink();

                var layoutModel = model.Layout ?? _contextFactory.CreateLayoutModel(currentContentLink, filterContext.RequestContext);

                /*
                var layoutController = filterContext.Controller as IModifyLayout;
                if (layoutController != null)
                {
                    layoutController.ModifyLayout(layoutModel);
                }
                */

                model.Layout = layoutModel;

                //if (model.Section == null)
                //{
                 //   model.Section = _contextFactory.GetSection(currentContentLink);
                //}
                
            }
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}