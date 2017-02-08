using EPiServer.Data.Dynamic;
using System.Collections.Generic;

namespace BaseSite.Business.ScheduledJobs.Adaptors
{
    [EPiServerDataStore(
       StoreName = "ScheduledJobParameters",
       AutomaticallyCreateStore = true,
       AutomaticallyRemapStore = true
   )]
    public class ScheduledJobParameters
    {
        public string PluginId { get; set; }
        public Dictionary<string, object> PersistedValues { get; set; }
    }
}