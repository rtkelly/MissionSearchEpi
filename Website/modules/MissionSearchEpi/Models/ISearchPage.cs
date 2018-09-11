
using EPiServer.Core;

namespace MissionSearchEpi
{
    public interface ISearchPage
    {
        XhtmlString NoResults { get; set; }

        int PageSize { get; set; }

        ContentArea QueryOptions { get; set; }
        
        ContentArea Facets { get; set; }
        
        ContentArea Sort { get; set; }
        
        ContentArea SuggestedResults { get; set; }
                

        //IBoostSettings BoostSettings { get; set; }


    }
}