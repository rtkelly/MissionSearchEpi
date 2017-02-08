using EPiServer.Shell;

namespace MissionSearchEpi.UI.ViewModels
{
    public class SearchAdminViewModelBase
    {
        public string GetShellPath(string path)
        {
            var shellPath = Paths.ToShellClientResource("");

            return shellPath + path;
        }
    }
}