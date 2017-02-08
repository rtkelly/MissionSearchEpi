using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseSite.Models.ViewModels
{
    public class PageViewModel<T> : IPageViewModel<T> where T : BasePage
    {
        public PageViewModel(T currentPage)
        {
            CurrentPage = currentPage;
        }

        public T CurrentPage { get; private set; }
        public LayoutModel Layout { get; set; }
        
    }

    public static class PageViewModel
    {
        public static PageViewModel<T> Create<T>(T page) where T : BasePage
        {
            return new PageViewModel<T>(page);
        }
    }
}