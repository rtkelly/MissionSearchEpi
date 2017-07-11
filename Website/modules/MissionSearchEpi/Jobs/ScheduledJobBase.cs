using EPiServer;
using EPiServer.Scheduler;
using System;

namespace MissionSearchEpi.Jobs
{
    public class MissionScheduledJobBase : ScheduledJobBase
    {
        protected bool StopSignaled;
        protected IContentRepository Repository;
        
        public MissionScheduledJobBase()
        {
            IsStoppable = true;
            Repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            StopSignaled = true;
        }


        public override string Execute()
        {
            throw new NotImplementedException();
        }

        /*
        protected Dictionary<string, object> LoadJobParameters(string typeName, string assemblyName)
        {
            var descriptor = PlugInDescriptor.Load(typeName, assemblyName);
            var store = typeof(ScheduledJobParameters).GetStore();

            return store.LoadPersistedValuesFor(descriptor.ID.ToString(CultureInfo.InvariantCulture));
        }

        protected void SaveJobParameters(Dictionary<string, object> controls, string typeName, string assemblyName)
        {
            var descriptor = PlugInDescriptor.Load(typeName, assemblyName);
            var store = typeof(ScheduledJobParameters).GetStore();

            var pluginId = descriptor.ID.ToString(CultureInfo.InvariantCulture);

            store.RemovePersistedValuesFor(pluginId);

            store.Save(
                new ScheduledJobParameters
                {
                    PluginId = pluginId,
                    PersistedValues = controls
                });
            
            
        }
         * */
    }
}