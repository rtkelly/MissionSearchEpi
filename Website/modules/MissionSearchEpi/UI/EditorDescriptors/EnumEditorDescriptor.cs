using EPiServer.Framework.Localization;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace MissionSearchEpi.UI.EditorDescriptors
{
    public class EnumEditorDescriptor<TEnum> : EditorDescriptor
    {
        public override void ModifyMetadata(
            ExtendedMetadata metadata,
            IEnumerable<Attribute> attributes)
        {
            SelectionFactoryType = typeof(EnumSelectionFactory<TEnum>);

            ClientEditingClass =
                "epi-cms/contentediting/editors/SelectionEditor";

            base.ModifyMetadata(metadata, attributes);
        }
    }

    public class EnumSelectionFactory<TEnum> : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var values = Enum.GetValues(typeof(TEnum));
            foreach (var value in values)
            {
                yield return new SelectItem
                {
                    Text = GetValueName(value),
                    Value = value
                };
            }
        }

        private string GetValueName(object value)
        {
            var staticName = Enum.GetName(typeof(TEnum), value) ?? "";

            string localizationPath = string.Format(
                "/property/enum/{0}/{1}",
                typeof(TEnum).Name.ToLowerInvariant(),
                staticName.ToLowerInvariant());

            string localizedName;
            if (LocalizationService.Current.TryGetString(
                localizationPath,
                out localizedName))
            {
                return localizedName;
            }

            return staticName;
        }
    }


}