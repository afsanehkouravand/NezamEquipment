using System.Collections.Generic;
using System.Reflection;

namespace MyCommon.Helpers.Form
{
    public class FormItem
    {
        public FormItem()
        {
            HtmlAttributes = new Dictionary<string, string>();
            Child = new Dictionary<string, string>();
            ChildGroup = new Dictionary<string, IDictionary<string, string>>();
        }

        internal string Label { get; set; }
        internal string Tag { get; set; }
        internal string Type { get; set; }
        internal string Name { get; set; }
        internal string Value { get; set; }
        internal bool Disabeld { get; set; }
        internal string Class { get; set; }
        internal string Style { get; set; }
        internal IDictionary<string, string> Child { get; set; }
        internal IDictionary<string, IDictionary<string, string>> ChildGroup { get; set; }
        internal IDictionary<string, string> HtmlAttributes { get; set; }
        internal int ColSize { get; set; }

        internal bool DataVal { get; set; }
        internal string DataValRequired { get; set; }

        internal string Text { get; set; }

        internal PropertyInfo PropertyInfo { get; set; }

        internal IList<string> DataList { get; set; }

        internal bool UseChooseOption { get; set; }

        internal bool IsCaptcha { get; set; }
        internal string CaptchaName { get; set; }
    }
}