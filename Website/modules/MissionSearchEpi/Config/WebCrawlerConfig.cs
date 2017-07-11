using MissionSearch.Crawlers;
using MissionSearchEpi.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MissionSearchEpi.Config
{
    public class WebCrawlerConfig
    {
        // TO DO: Make this path configurable
        static string configPath = "/App_Data/webcrawlerconfig.json";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<WebCrawlJobConfig> GetConfigData()
        {
            var list = new List<WebCrawlJobConfig>();

            var path = HostingEnvironment.MapPath(configPath);

            if (string.IsNullOrEmpty(path))
                throw new Exception("Web Crawler Config path not defined.");

            if (!File.Exists(path))
                return list;

            var jobListStr = File.ReadAllLines(path);
            
            foreach(var jobStr in jobListStr)
            {
                list.Add(JsonConvert.DeserializeObject<WebCrawlJobConfig>(jobStr));
            }

            return list.OrderBy(p => p.SourceId).ToList();
        }

        public static WebCrawlJobConfig GetJobConfigData(int sourceId)
        {
            var configData = GetConfigData();

            var existing = configData.FirstOrDefault(p => p.SourceId == sourceId);

            if (existing != null)
            {
                return existing;
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public static void SaveConfigData(WebCrawlJobConfig data)
        {
            var configData = GetConfigData();

            var existing = configData.FirstOrDefault(p => p.SourceId == data.SourceId);
            
            if(existing != null)
            {
                configData = configData.Where(p => p.SourceId != data.SourceId).ToList();
            }
            
            configData.Add(data);
            
            var path = HostingEnvironment.MapPath(configPath);

            if (string.IsNullOrEmpty(path))
                throw new Exception("Mission Config path not defined.");

            using (var r = new StreamWriter(path))
            {
                foreach(var job in configData)
                {
                    var json = JsonConvert.SerializeObject(job);
                    r.WriteLine(json);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceId"></param>
        public static void DeleteConfigData(int sourceId)
        {
            var configData = GetConfigData();

            var toDelete = configData.FirstOrDefault(p => p.SourceId == sourceId);

            if (toDelete != null)
            {
                configData = configData.Where(p => p.SourceId != sourceId).ToList();


                var path = HostingEnvironment.MapPath(configPath);

                if (string.IsNullOrEmpty(path))
                    throw new Exception("Mission Config path not defined.");

                using (var r = new StreamWriter(path))
                {
                    foreach (var job in configData)
                    {
                        var json = JsonConvert.SerializeObject(job);
                        r.WriteLine(json);
                    }
                }
            }

        }
    }
}