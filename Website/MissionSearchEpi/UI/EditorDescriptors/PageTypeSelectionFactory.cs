using EPiServer.Shell.ObjectEditing;
using MissionSearch;
using MissionSearch.Search.Facets;
using System.Collections.Generic;
using System.Linq;

namespace MissionSearchEpi.EditorDescriptors
{
    public class PageTypeSelectionFactory : ISelectionFactory
    {
        string srchFieldName = "contenttype";

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var facets = new List<IFacet>();

            facets.Add(new FieldFacet(srchFieldName));

            var resp = SearchFactory<SearchDocument>.SearchClient.Search(new SearchRequest()
            {
                QueryText = "*",
                PageSize = 1,
                //RefinementType = RefinementType.SingleSelect,
                Facets = facets,
            });

            var refinement = resp.Refinements.FirstOrDefault(r => r.Name == srchFieldName);
            var items = (refinement != null) ? refinement.Items : new List<RefinementItem>();

            var categories = new List<string>();

            items.ForEach(item => categories.Add(item.Value.Replace("\"", "")));

            yield return new SelectItem
            {
                Text = "--None--",
                Value = "None",
            };

            foreach (var category in categories.OrderBy(p => p))
            {
               
                yield return new SelectItem
                {
                    Text = category,
                    Value = category,
                };
            }
        }

     
    }
}