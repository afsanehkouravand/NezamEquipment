using System.Collections.Generic;
using System.Linq;

namespace MyCommon.Helpers.Grid
{
    public static class GridColumnExtension
    {
        public static GridColumn SetIsFinancial(this GridColumn data, bool withRialExt = true)
        {
            data.IsFinancial = true;
            data.IsFinancialWithRialExt = withRialExt;
            return data;
        }
        public static GridColumn SetCustomTemplate(this GridColumn data, string template)
        {
            data.IsCustomTemplate = true;
            data.IsCustomTemplateHtml = template;
            return data;
        }
        public static GridColumn SetIsList(this GridColumn data, string propertyName)
        {
            data.IsList = true;
            data.IsListPropertyName = propertyName;
            return data;
        }
        public static GridColumn SetIsBoolean(this GridColumn data, string onTrueTemplate, string onFalseTemplate)
        {
            data.IsBoolean = true;
            data.IsBooleanOnTrueTemplate = onTrueTemplate;
            data.IsBooleanOnFalseTemplate = onFalseTemplate;
            return data;
        }
        public static GridColumn ReplaceValueWith(this GridColumn data, string value, string replaceWith)
        {
            if (data.ReplaceValueWiths == null)
                data.ReplaceValueWiths = new Dictionary<string, string>();

            data.IsReplaceValueWith = true;

            if (data.ReplaceValueWiths.Any(x=>x.Key == value))
            {
                data.ReplaceValueWiths[value] = replaceWith;
            }
            else
            {
                data.ReplaceValueWiths.Add(value, replaceWith);
            }

            return data;
        }
        public static GridColumn ReplaceValueWith(this GridColumn data, IDictionary<string, string> valueReplaceWith)
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
        public static GridColumn SetLabel(this GridColumn data, string label)
        {
            data.Label = label;
            return data;
        }
    }
}
