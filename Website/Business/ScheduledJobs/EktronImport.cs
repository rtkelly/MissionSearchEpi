using System;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.BaseLibrary.Scheduling;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using BaseSite.Models.Pages;
using EPiServer.ServiceLocation;
using EPiServer;
using EPiServer.DataAbstraction;
using System.Text.RegularExpressions;
using BaseSite.Business.Util;
using EPiServer.Scheduler;


namespace BaseSite.Business.Import
{
    [ScheduledPlugIn(DisplayName = "Ektron Import")]
    public class EktronImport : ScheduledJobBase
    {
        private bool _stopSignaled;

        IContentRepository _contentRepository;
        int _articleTypeId;
        int _newsPageTypeId;

        public EktronImport()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged("Starting Ektron Importer");
            
            //Add implementation

            try
            {
                _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();

                _articleTypeId = contentTypeRepository.Load<ArticlePage>().ID;
                _newsPageTypeId = contentTypeRepository.Load<NewsPage>().ID;

                var parent = new ContentReference(3958);

                if (parent == null)
                    return "Error container page not found.";

                var dt = new DataTable();

                //var ektronConnStr = "server=CCIMSQL-Cloud.cloudapp.net;database=CCIM;Integrated Security=false;user=CCIMUser;pwd=CCIMUser123;";
                //var ektronConnStr = "server=CFFSQL-Cloud.cloudapp.net;database=CFF;Integrated Security=FALSE;user=cffuser;pwd=Syscom2015;";
                var ektronConnStr = "server=SYSCOMSQL-VM;database=CMS-Production;Integrated Security=false;user=soa1;pwd=123456;";

                int cnt = 0;
                int errorcnt = 0;

                using (SqlConnection connection = new SqlConnection(ektronConnStr))
                {
                    var command = new SqlCommand("select * from content where xml_config_id = 8", connection);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    dt.Load(command.ExecuteReader());

                    connection.Close();

                    var rows = dt.Rows.Cast<DataRow>().ToList();
                    
                    foreach (var row in rows)
                    {
                        try
                        {
                            //CreateArticle(row, parent);
                            CreateNewsItems2(row, parent);
                            cnt++;
                        }
                        catch
                        {
                            errorcnt++;
                        }
                    }

                }

                //For long running jobs periodically check if stop is signaled and if so stop execution
                if (_stopSignaled)
                {
                    return "Stop of job was called";
                }

                return string.Format("Import Finished. Items Imported: Total Added: {0} Total Failed: {1}", cnt, errorcnt);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private void CreateArticle(DataRow row, ContentReference parent)
        {
            var html = row["content_html"].ToString();

            var parser = new XmlParser(html);

            var myPage = _contentRepository.GetDefault<ArticlePage>(parent, _articleTypeId);

            myPage.Name = row["content_title"].ToString();
            myPage.PageTitle = row["content_title"].ToString();
            myPage.SubTitle = parser.ParseString("/root/SubTitle");
            myPage.Intro = parser.ParseXHtmlString("/root/Introduction");
            myPage.MainBody = parser.ParseXHtmlString("/root/Article");
            //myPage.URLSegment = EPiServer.Web.UrlSegment.CreateUrlSegment(myPage);
            myPage.Summary = StripHtml(row["content_teaser"].ToString());
            
            _contentRepository.Save(myPage, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

        }


        private void CreateNewsItems2(DataRow row, ContentReference parent)
        {
            var html = row["content_html"].ToString();

            var parser = new XmlParser(html);

            var myPage = _contentRepository.GetDefault<NewsPage>(parent, _newsPageTypeId);

            myPage.Name = row["content_title"].ToString();
            myPage.PageTitle = parser.ParseString("/PressReleases/Title");
            myPage.Summary = StripHtml(row["content_teaser"].ToString());

            myPage.Intro = parser.ParseXHtmlString("/PressReleases/Summary");
            myPage.MainBody = parser.ParseXHtmlString("/PressReleases/Body");
            myPage.PublishedDate = BuildDate(2012);

            //myPage.URLSegment = EPiServer.Web.UrlSegment.CreateUrlSegment(myPage);
            
            _contentRepository.Save(myPage, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

        }

        private void CreateNewsItems(DataRow row, ContentReference parent)
        {
            var html = row["content_html"].ToString();

            var parser = new XmlParser(html);

            var myPage = _contentRepository.GetDefault<NewsPage>(parent, _newsPageTypeId);

            myPage.Name = row["content_title"].ToString();
            myPage.PageTitle = row["content_title"].ToString();
            myPage.Intro = parser.ParseXHtmlString("/root/Introduction");
            myPage.MainBody = parser.ParseXHtmlString("/root/Body");
            //myPage.URLSegment = EPiServer.Web.UrlSegment.CreateUrlSegment(myPage);
            myPage.Summary = StripHtml(row["content_teaser"].ToString());
            myPage.PublishedDate = BuildDate(2014);

            _contentRepository.Save(myPage, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);

        }


        private DateTime BuildDate(int year)
        {
            var random = new Random();

            var month = random.Next(1, 12);
            var day = random.Next(1, 28);

            return new DateTime(year, month, day);
        }

        private string StripHtml(string html)
        {
            return Regex.Replace(html, @"<[^>]*>", String.Empty).Trim();
            
        }
    }
}
