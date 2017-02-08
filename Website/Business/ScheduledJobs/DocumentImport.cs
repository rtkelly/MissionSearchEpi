using System;
using System.Linq;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.BaseLibrary.Scheduling;
using EPiServer;
using System.IO;
using EPiServer.Framework.Blobs;
using EPiServer.ServiceLocation;
using EPiServer.DataAccess;
using EPiServer.Web;
using BaseSite.Models.Media;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using EPiServer.Scheduler;

namespace BaseSite.Business.ScheduledJobs
{
    [ScheduledPlugIn(DisplayName = "Document Import")]
    public class DocumentImport : ScheduledJobBase
    {
        private bool _stopSignaled;
        IContentRepository _contentRepository;
        BlobFactory _blobFactory;

        //private string _mediaPath = @"D:\development\data";
        //private string _mediaPathRoot = @"D:\development\data\ccimpdfs\";
        private string _mediaPathRoot = @"C:\webroot\EpiSearch\App_Data\soaassets\";

        public DocumentImport()
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
            OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));

            //Add implementation

            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _blobFactory = ServiceLocator.Current.GetInstance<BlobFactory>();

            var rootFolders = _contentRepository.GetChildren<ContentFolder>(SiteDefinition.Current.GlobalAssetsRoot);
            var rootFolder = rootFolders.FirstOrDefault(f => f.Name == "pdfs3");

            if (rootFolder == null)
                return "failed";

            var documents = GetDocuments();

            var total = 0;
            var errorcnt = 0;

            foreach(var docRow in documents)
            {
                try
                {
                    var ext = Path.GetExtension(docRow["name"].ToString());
                    var path = docRow["pubFolderPath"].ToString().Replace("/", "\\");
                    var title = docRow["content_title"].ToString();
                    var teaser = docRow["content_teaser"].ToString();
                    var date = docRow["last_edit_date"] as DateTime?;
                    
                    var filepath = string.Format("{0}{1}{2}{3}", _mediaPathRoot, path, docRow["asset_id"], ext);

                    if (File.Exists(filepath))
                    {
                        AddMedia(title, filepath, teaser, date, rootFolder);
                        total++;
                    }
                }
                catch
                {
                    errorcnt++;
                }
            }

            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return string.Format("Import Finished. Documents Imported: Total Added: {0} Total Failed: {1}", total, errorcnt);
        }


        private List<DataRow> GetDocuments()
        {
            var rows = new List<DataRow>();
            //var ektronConnStr = "server=192.168.120.27;database=CCIM;Integrated Security=false;user=brightfind;pwd=Brightfind2015!";
            var ektronConnStr = "server=SYSCOMSQL-VM;database=CMS-Production;Integrated Security=false;user=soa1;pwd=123456;";

            using (SqlConnection connection = new SqlConnection(ektronConnStr))
            {
                //var sql = "SELECT * FROM dbo.content as c join AssetDataTable as a on c.asset_id = a.id where content_type = 102 and pubfolderpath like '0/142%'";
                var sql = "SELECT * FROM dbo.content as c join AssetDataTable as a on c.asset_id = a.id where content_type = 102 and pubfolderpath like '0/169%'";
                
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;
                connection.Open();

                var dt = new DataTable();

                dt.Load(command.ExecuteReader());

                rows = dt.Rows.Cast<DataRow>().ToList();
            }

            return rows;
        }

        private void AddMedia(string title, string fullpath, string teaser, DateTime? date, ContentFolder rootFolder)
        {
            var mediaFile = _contentRepository.GetDefault<DocumentFile>(rootFolder.ContentLink);
            mediaFile.Name = title;
            mediaFile.Description = StripHtml(teaser);
            mediaFile.PubDate = date == null ? DateTime.Today : date.Value;

            var extension = Path.GetExtension(fullpath);

            var blob = _blobFactory.CreateBlob(mediaFile.BinaryDataContainer, extension);

            var fileData = File.ReadAllBytes(fullpath);
            
            using (var s = blob.OpenWrite())
            {
                var w = new BinaryWriter(s);
                w.Write(fileData);
                w.Flush();
            }

            mediaFile.BinaryData = blob; 
            
            _contentRepository.Save(mediaFile, SaveAction.Publish);
        }

        private string StripHtml(string html)
        {
            return Regex.Replace(HttpUtility.HtmlDecode(html), @"<[^>]*>", String.Empty).Trim();

        }
         
    }
}
