using System.Linq;
using System.Web;
using System.Web.WebPages;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using BaseSite.Business.Channels;

namespace BaseSite.Business.Intialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DisplayModeConfig : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var mobileChannelDisplayMode = new DefaultDisplayMode("mobile")
            {
                ContextCondition = IsMobileDisplayModeActive
            };
            DisplayModeProvider.Instance.Modes.Insert(0, mobileChannelDisplayMode);
        }

        private static bool IsMobileDisplayModeActive(HttpContextBase httpContext)
        {
            if (httpContext.GetOverriddenBrowser().IsMobileDevice)
            {
                return true;
            }
            var displayChannelService = ServiceLocator.Current.GetInstance<DisplayChannelService>();
            return displayChannelService.GetActiveChannels(httpContext).Any(x => x.ChannelName == MobileChannel.Name);
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}