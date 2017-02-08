using System;
using System.Configuration;
using System.Web.Mvc;
using MissionSearch;
using MissionSearchEpi;
using System.Web.Routing;
using MissionSearchEpi.Config;

namespace BaseSite
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)

            MissionRegistration.RegisterAll();
        }


       
    }
}