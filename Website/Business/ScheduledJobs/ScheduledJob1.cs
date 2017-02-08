using System;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.BaseLibrary.Scheduling;
using BaseSite.Models;
using BaseSite.Models.Pages;
using System.Collections.Generic;
using EPiServer;
using EPiServer.ServiceLocation;
using EPiServer.Scheduler;

namespace BaseSite.Business.ScheduledJobs
{
    [ScheduledPlugIn(DisplayName = "ScheduledJob1")]
    public class ScheduledJob1 : ScheduledJobBase
    {
        private bool _stopSignaled;
        protected IContentRepository Repository;

        int totalcnt;
        int errorcnt;

        public ScheduledJob1() 
        {

        }

        

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            Repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));

            totalcnt = 0;
            errorcnt = 0;

            var ids = new List<int>() { 3958, 3420, 3238, 2996, 148, 165, 449, 574, 1301, 185 };
            
            foreach(var id in ids)
            {
                UpdateContent(id);
            }

             //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return string.Format("Total Updated {0}. Total Errors {1}", totalcnt, errorcnt);

        }

        private void UpdateContent(int id)
        {
            var parent = new ContentReference(id);
            
            var childPages = Repository.GetChildren<ArticlePage>(parent);
            var parentPage = Repository.Get<ContainerPage>(parent);

            foreach(var child in childPages)
            {
                try
                {
                    var page = (ArticlePage)child.CreateWritableClone();
                    
                    page.PublishedDate = BuildDate(int.Parse(parentPage.Name));
                    //page.PublicationName = string.Format("{0:MMM} {1}", page.PubDate, page.PubDate.Year);
                    //page.Author = BuildAuthor();

                    //if (page.Name.Contains("SOA") || page.Name.Contains("SOA") || page.Summary.Contains("SOA"))
                    //{
                    /*
                    page = ScrubNewsPage(page, "SOA", "BGGL");
                    page = ScrubNewsPage(page, "Society of Actuaries", "BGGL");
                    page = ScrubNewsPage(page, "CF", "Plumbers");
                    page = ScrubNewsPage(page, "Cystic Fibrosis", "Plumbers");
                    */

                    Repository.Save(page, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
                    //}

                    totalcnt++;
                }
                catch
                {
                    errorcnt++;
                }
            }




           
        }


        private BasePage ScrubBasePage(BasePage page, string scrub, string replace)
        {
            page.Name = page.Name.Replace(scrub, replace);

            return page;
        }

        private NewsPage ScrubNewsPage(NewsPage page, string scrub, string replace)
        {
            page.Name = page.Name.Replace(scrub, replace);
            
            if(page.Summary != null)
                page.Summary = page.Summary.Replace(scrub, replace);

            if (page.PageTitle != null)
                page.PageTitle = page.PageTitle.Replace(scrub, replace);

            if (page.MainBody != null)
                page.MainBody = new XhtmlString(page.MainBody.ToHtmlString().Replace(scrub, replace));

            return page;
        }

        private string BuildAuthor()
        {
            string[] Authors = { "Hans Christian Andersen", "Hans Andersen", "Robert Louis Stevenson", "Robert Stevenson", "Edgar Allan Poe", "Robert Frost" };
            var random = new Random();

            var name = Authors[random.Next(0, 5)];
            
            return name;
        }

        private DateTime BuildDate(int year)
        {
            var random = new Random();

            var month = random.Next(1, 12);
            var day = random.Next(1, 28);
            //year = random.Next(2010, 2015);

            return new DateTime(year, month, day);
        }
    }
}
