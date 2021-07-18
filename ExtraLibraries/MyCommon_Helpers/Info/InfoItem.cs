using System.Collections.Generic;
using System.Reflection;

namespace MyCommon.Helpers.Info
{
    public class InfoItem
    {
        internal PropertyInfo PropertyInfo { get; set; }

        internal string PropertyName { get; set; }
        internal string Label { get; set; }
        internal object Value { get; set; }

        internal bool IsFinancial { get; set; }
        internal bool IsFinancialWithRialExt { get; set; }

        internal bool IsCustomTemplate { get; set; }
        internal string IsCustomTemplateHtml { get; set; }

        internal bool IsList { get; set; }
        internal string IsListPropertyName { get; set; }

        internal bool IsBoolean { get; set; }
        internal string IsBooleanOnTrueTemplate { get; set; }
        internal string IsBooleanOnFalseTemplate { get; set; }

        internal int? ColSize { get; set; }
        internal int? Colspan { get; set; }

        internal bool IsEmptyLine { get; set; }
        internal bool IsTitle { get; set; }

        internal bool PopOver { get; set; }
        internal bool PopOverLtr { get; set; }
        internal string PopOverTitle { get; set; }
        internal string PopOverContent { get; set; }
        internal string PopOverPlacement { get; set; }


        internal bool IsReplaceValueWith { get; set; }
        internal IDictionary<string, string> ReplaceValueWiths { get; set; }

    }
}