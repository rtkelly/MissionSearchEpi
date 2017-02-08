using MissionSearch.Clients.Solr;
using System.Collections.Generic;

namespace MissionSearchEpi.UI.ViewModels
{
    public class SynonymsAdminViewModel : SearchAdminViewModelBase
    {
        public List<Synonym> Synonyms { get; set; }

    }
}