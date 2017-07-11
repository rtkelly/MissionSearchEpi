using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Hosting;


namespace MissionSearchEpi.Config
{
    public class MissionConfig
    {
        // TO DO: Make this path configurable
        static string configPath = "/App_Data/missionconfig.json";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MissionConfigData GetConfigData()
        {
            var path = HostingEnvironment.MapPath(configPath);

            if (string.IsNullOrEmpty(path))
                throw new Exception("Mission Config path not defined.");

            if (!File.Exists(path))
                return new MissionConfigData();

            using (var r = new StreamReader(path))
            {
                var json = r.ReadToEnd();
                var config = JsonConvert.DeserializeObject<MissionConfigData>(json);

                return config ?? new MissionConfigData();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public static void SaveConfigData(MissionConfigData data)
        {
            var path = HostingEnvironment.MapPath(configPath);

            if(string.IsNullOrEmpty(path))
                throw new Exception("Mission Config path not defined.");

            var json = JsonConvert.SerializeObject(data);
            
            using (var r = new StreamWriter(path))
            {
                r.Write(json);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="now"></param>
        /// <param name="duration"></param>
        public static void SetLastAssetCrawlDate(MissionConfigData configData, DateTime now, TimeSpan duration)
        {
            configData.LastAssetCrawledDate = now;
            configData.LastAssetCrawlDuration = duration;
            SaveConfigData(configData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="now"></param>
        /// <param name="duration"></param>
        public static void SetLastContentCrawlDate(MissionConfigData configData, DateTime now, TimeSpan duration)
        {
            configData.LastContentCrawledDate = now;
            configData.LastContentCrawlDuration = duration;
            SaveConfigData(configData);
        }
    }
}