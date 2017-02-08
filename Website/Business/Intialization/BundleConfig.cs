using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using System.Web.Optimization;

namespace BaseSite.Business.Intialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class BundleConfig : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if (context.HostType == HostType.WebApplication)
            {
                RegisterBundles(BundleTable.Bundles);
            }
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new ScriptBundle("~/bundles/js")
                    .Include("~/Static/js/bootstrap.js")
                    .Include("~/Static/js/searchpage.js")
                    //.Include("~/Static/js/jquery-ui.min.js")
                                        
                );

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Static/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/bootstrap-responsive.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/style.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/starter-template.css", new CssRewriteUrlTransform())
                //.Include("~/Static/css/theme.css", new CssRewriteUrlTransform())
                );
             
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}