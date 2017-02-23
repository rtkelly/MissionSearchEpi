using EPiServer.Shell.ObjectEditing;
using MissionSearch;
using MissionSearch.Search.Facets;
using System.Collections.Generic;
using System.Linq;

namespace MissionSearchEpi.EditorDescriptors
{
    public class CmsPathSelectionFactory : ISelectionFactory
    {
        string pathsFieldName = "path";

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var facets = new List<IFacet>();
            
            facets.Add(new FieldFacet(pathsFieldName));

            var resp = SearchFactory<SearchDocument>.SearchClient.Search(new SearchRequest()
            {
                   QueryText = "*",
                   PageSize = 1,
                   //RefinementType = RefinementType.SingleSelect,
                   Facets = facets,
            });

            var refinement = resp.Refinements.FirstOrDefault(r => r.Name == pathsFieldName);
            var items = (refinement != null) ? refinement.Items : new List<RefinementItem>();

            var paths = new List<string>();

            foreach (var result in items)
            {
                var parts = result.Value.Replace("\"", "").Split('/');
                var lastPath = "";

                foreach(var part in parts)
                {
                    if (part == "")
                        continue;

                    var path = string.Format("{0}/{1}", lastPath, part);

                    if (!paths.Contains(path))
                        paths.Add(path);
                    lastPath = path;
                }
            }

            yield return new SelectItem
            {
                Text = "--None--",
                Value = "None",
            };

            foreach (var path in paths.OrderBy(p => p))
            {
                yield return new SelectItem
                {
                    Text = path,
                    Value = path,
                };
            }
             
        }

     
    }
}