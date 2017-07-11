using System;
using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System.Linq;
using MissionSearch.Attributes;

namespace MissionSearchEpi.UI.EditorDescriptors
{

   /// <summary>
   /// 
   /// </summary>
   /// <typeparam name="TDoc"></typeparam>
   /// <typeparam name="T"></typeparam>
    public class SearchDocTaggedSelectionFactory<TDoc, T> : ISelectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            Type docType = typeof(TDoc);
            Type dataType = typeof(T);
            Type displayNameType = typeof(DisplayName);

            var properties = docType.GetProperties();

            var selectItems = new List<SelectItem>();

            foreach (var prop in properties.OrderBy(p => p.Name))
            {
                if (prop.Name == "highlightsummary" || prop.Name == "content" || prop.Name == "id")
                    continue;

                var attrbs = prop.GetCustomAttributes(true);

                if (attrbs.Any(f => f.GetType().Name == dataType.Name))
                {
                    var propName = prop.Name.First().ToString().ToUpper() + String.Join("", prop.Name.Skip(1));
                    var displayName = attrbs.FirstOrDefault(f => f.GetType().Name == displayNameType.Name) as DisplayName;
                    var name = (displayName != null) ? displayName.FieldName : propName;

                    selectItems.Add(new SelectItem
                    {
                        Text = name,
                        Value = prop.Name,
                    });
                }

            }
            
            foreach (var item in selectItems.OrderBy(i => i.Text))
            {
                yield return item;
            }
        }
    }

}