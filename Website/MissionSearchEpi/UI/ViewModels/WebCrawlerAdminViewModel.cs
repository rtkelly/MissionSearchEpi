using MissionSearch;
using MissionSearch.Crawlers;
using MissionSearchEpi.Config;
using MissionSearchEpi.Jobs;
using MissionSearchEpi.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissionSearchEpi.UI.ViewModels
{
    public class WebCrawlerAdminViewModel : SearchAdminViewModelBase
    {

        public List<WebCrawlJobConfig> GetWebCrawlJobs()
        {
            return WebCrawlerConfig.GetConfigData();
        }


        public string GetStatus(MissionSearchEpi.Config.WebCrawlJobConfig.ScheduleIntervalType statusType, int interval, DateTime? lastRun)
        {
            if (lastRun == null && statusType != MissionSearchEpi.Config.WebCrawlJobConfig.ScheduleIntervalType.Inactive)
            {
                return "";
            }
        
            switch(statusType)
            {
                case MissionSearchEpi.Config.WebCrawlJobConfig.ScheduleIntervalType.Inactive:
                    return "Inactive";

                case MissionSearchEpi.Config.WebCrawlJobConfig.ScheduleIntervalType.Day:
                    return string.Format("{0:MM-dd-yyyy}", lastRun.Value.AddDays(interval));
                
                case MissionSearchEpi.Config.WebCrawlJobConfig.ScheduleIntervalType.Week:
                    return string.Format("{0:MM-dd-yyyy}", lastRun.Value.AddDays(interval*7));

                case MissionSearchEpi.Config.WebCrawlJobConfig.ScheduleIntervalType.Month:
                    return string.Format("{0:MM-dd-yyyy}", lastRun.Value.AddMonths(interval));
                
                default:
                    return "";
            }
        }


        public int GetCount(int sourceid)
        {
            var srchClient = SearchFactory<WebCrawlerSearchDoc>.SearchClient;
                        
            var result = srchClient.Search(string.Format("sourceid:{0}", sourceid));

            return result.TotalFound;
        }
    }
}