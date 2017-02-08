using EPiServer.Shell.ObjectEditing;
using MissionSearch.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MissionSearchEpi.EditorDescriptors
{
    public class SearchDocTypedSelectionFactory<DOC, T> : ISelectionFactory
    {
        /// <summary>
        /// retrieve a list of property name of properties of a given type of type DOC
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            Type docType = typeof(DOC);
            
            var properties = docType.GetProperties();

            Type displayNameType = typeof(DisplayName);

            var selectedTypeName = typeof(T).Name;

            foreach (var prop in properties.OrderBy(p => p.Name))
            {
                if(selectedTypeName == "String")
                {
                    if(prop.PropertyType.Name == "List`1")
                    {
                        if (!prop.PropertyType.GenericTypeArguments.Any())
                            continue;

                        if (prop.PropertyType.GenericTypeArguments[0].Name.ToLower() != "string")
                            continue;
                    }
                    else if (prop.PropertyType.Name.ToLower() != "string")
                        continue;
                }
                else if (prop.PropertyType.Name != selectedTypeName)
                    continue;

                if (prop.Name == "highlightsummary" || prop.Name == "content" || prop.Name == "id")
                    continue;
                
                var attrbs = prop.GetCustomAttributes(true);

                var propName = prop.Name.First().ToString().ToUpper() + String.Join("", prop.Name.Skip(1));
                var displayName = attrbs.FirstOrDefault(f => f.GetType().Name == displayNameType.Name) as DisplayName;
                var name = (displayName != null) ? displayName.FieldName : propName;

                yield return new SelectItem
                {
                    Text = name,
                    Value = prop.Name,
                };
            }
        }
    }

}