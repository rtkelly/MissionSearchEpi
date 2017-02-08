using MissionSearch;
using System.Collections.Generic;

namespace MissionSearchEpi.UI.ViewModels
{
    public class SuggestedQueriesAdminViewModel : SearchAdminViewModelBase
    {
        public string SearchServer { get; set; }
        
        public string SearchClient { get; set; }

        public string QueryTerm { get; set; }
        
        public long IndexTotal { get; set; }
        
        public List<string> Terms { get; set; }

        public Pagination PagingInfo { get; set; }

        public long TotalFound { get; set; }
    }
}