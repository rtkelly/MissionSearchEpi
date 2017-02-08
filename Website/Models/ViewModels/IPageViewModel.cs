using EPiServer.Core;

namespace BaseSite.Models.ViewModels
{
    
    public interface IPageViewModel<out T> where T : BasePage
    {
        T CurrentPage { get; }
        LayoutModel Layout { get; set; }
    }
}