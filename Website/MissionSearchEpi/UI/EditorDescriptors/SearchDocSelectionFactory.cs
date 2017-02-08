using EPiServer.Shell.ObjectEditing;
using MissionSearch.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MissionSearchEpi.EditorDescriptors
{
    public class SearchDocSelectionFactory<T> : ISelectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            Type docType = typeof(T);
            
            var properties = docType.GetProperties();

            Type displayNameType = typeof(DisplayName);

            foreach (var prop in properties.OrderBy(p => p.Name))
            {
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