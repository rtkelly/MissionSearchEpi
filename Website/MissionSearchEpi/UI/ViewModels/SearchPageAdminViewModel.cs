using MissionSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MissionSearchEpi.UI.ViewModels
{
    public class SearchPageAdminViewModel<T> : SearchAdminViewModelBase where T : ISearchDocument
    {
        public string QueryText { get; set; }

        public SearchResponse<T> Response { get; set;}

        public string ResultsString() { return string.Empty; }

        public bool IsCurrentSort(string sort) { return false; }
                
    }
}