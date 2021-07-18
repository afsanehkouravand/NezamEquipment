using System.Collections.Generic;
using System.Linq;

namespace MyCommon.Helpers.Info
{
    public static class InfoItemExtension
    {
        public static InfoItem Label(this InfoItem data, string label)
        {
            data.Label = label;
            return data;
        }
        public static InfoItem IsCustomTemplate(this InfoItem data, string template)
        {
            data.IsCustomTemplate = true;
            data.IsCustomTemplateHtml = template;
            return data;
        }
        public static InfoItem IsList(this InfoItem data, string name)
        {
            data.IsList = true;
            data.IsListPropertyName = name;
            return data;
        }
        public static InfoItem IsFinancial(this InfoItem data, bool withRialExt = true)
        {
            data.IsFinancial = true;
            data.IsFinancialWithRialExt = withRialExt;
            return data;
        }
        public static InfoItem SetPopOver(this InfoItem data, string title, string content, string placement = "top", bool? ltr = null)
        {
            data.PopOver = true;
            data.PopOverLtr = ltr ?? false;
            data.PopOverTitle = title;
            data.PopOverContent = content;
            data.PopOverPlacement = placement;
            return data;
        }
        public static InfoItem IsBoolean(this InfoItem data, string onTrue, string onFalse)
        {
            data.IsBoolean = true;
            data.IsBooleanOnTrueTemplate = onTrue;
            data.IsBooleanOnFalseTemplate = onFalse;
            return data;
        }
        public static InfoItem ReplaceValueWith(this InfoItem data, string value, string replaceWith)
        {
            if (data.ReplaceValueWiths == null)
                data.ReplaceValueWiths = new Dictionary<string, string>();

            data.IsReplaceValueWith = true;

            if (data.ReplaceValueWiths.Any(x => x.Key == value))
            {
                data.ReplaceValueWiths[value] = replaceWith;
            }
            else
            {
                data.ReplaceValueWiths.Add(value, replaceWith);
            }

            return data;
        }
        public static InfoItem ReplaceValueWith(this InfoItem data, IDictionary<string, string> valueReplaceWith)
        {
            if (data.ReplaceValueWiths == null)
                data.ReplaceValueWiths = new Dictionary<string, string>();

            data.IsReplaceValueWith = true;

            foreach (var item in valueReplaceWith)
            {
                if (data.ReplaceValueWiths.Any(x => x.Key == item.Key))
                {
                    data.ReplaceValueWiths[item.Key] = item.Value;
                }
                else
                {
                    data.ReplaceValueWiths.Add(item.Key, item.Value);
                }
            }

            return data;
        }

    }
}