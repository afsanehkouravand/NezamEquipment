using System.Collections.Generic;
using System.Reflection;

namespace MyCommon.Helpers.Grid
{
    public class GridColumn
    {
        internal PropertyInfo PropertyInfo { get; set; }

        internal string PropertyName { get; set; }
        internal string Label { get; set; }
        internal string Value { get; set; }


        internal bool IsList { get; set; }
        internal string IsListPropertyName { get; set; }


        internal bool IsBoolean { get; set; }
        internal string IsBooleanOnTrueTemplate { get; set; }
        internal string IsBooleanOnFalseTemplate { get; set; }

        internal bool IsFinancial { get; set; }
        internal bool IsFinancialWithRialExt { get; set; }

        internal bool IsCustomTemplate { get; set; }
        internal string IsCustomTemplateHtml { get; set; }


        internal bool IsHtml { get; set; }
        internal GridHtml IsHtmlTemplate { get; set; }


        internal bool IsColumnCounter { get; set; }

        internal bool IsColumnForButton { get; set; }

        internal bool IsHiddenInput { get; set; }

        internal bool IsReplaceValueWith { get; set; }
        internal IDictionary<string, string> ReplaceValueWiths { get; set; }

    }
}