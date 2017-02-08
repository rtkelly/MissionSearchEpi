using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using System.Web.Mvc;
using EPiServer.ServiceLocation;

namespace BaseSite.Business.Intialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class FilterConfig : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            //Add initialization logic, this method is called once after CMS has been initialized
            GlobalFilters.Filters.Add(ServiceLocator.Current.GetInstance<PageContextActionFilter>());
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}