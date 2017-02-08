using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseSite.Models.Pages;
using MissionSearch;
using BaseSite.Models.ViewModels;
using MissionSearchEpi;
using EPiServer.Core;
using MissionSearchEpi.Models;

namespace BaseSite.Models.ViewModels
{
    public class SearchViewModel<T> : PageViewModel<SearchPage> where T : ISearchDocument 
    {
        public string QueryText { get; set; }

        public bool DebugQuery { get; set; }
        
        public SearchResponse<T> Response { get; set; }
        
        public String CurrentSort { get; set; }

        public IEnumerable<T> SuggestedResults { get; set; }

        private Pagination _pagingInfo { get; set; }

        public Pagination PagingInfo { get
            {
                if(_pagingInfo == null)
                {
                    if (Response == null)
                        return null;

                    _pagingInfo = Response.BuildPagination();
                }

                return _pagingInfo;
            }
        }

        public SearchViewModel(SearchPage currentPage)
            : base(currentPage)
        {
                    
        }

        public string IsCurrentSort(string fieldname)
        {
            return fieldname == CurrentSort ? "selected" : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ResultsString()
        {
            return string.Format("{0} - {1} of {2}", PagingInfo.StartRow, PagingInfo.EndRow, PagingInfo.TotalRows);
        }

        
        public string BuildPostBackUrl(string queryString)
        {
            return string.Format("{0}{1}", PostBackUrl, queryString);
        }

       
        private string PostBackUrl
        {
            get
            {
                var url = string.Format("{0}&q={1}", CurrentPage.LinkURL, HttpContext.Current.Request["q"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["sort"]))
                {
                    url = string.Format("{0}&sort={1}", url, HttpContext.Current.Request["sort"]);
                }

                return url;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public string BuildPaginationUrl(int page)
        {
            var refiments = (!string.IsNullOrEmpty(HttpContext.Current.Request["ref"])) ? string.Format("&ref={0}", HttpContext.Current.Request["ref"]) : "";

            return string.Format("{0}&page={1}{2}", PostBackUrl, page, refiments);
        }

    }


}