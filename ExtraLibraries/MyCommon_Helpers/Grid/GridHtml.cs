using System.Collections.Generic;

namespace MyCommon.Helpers.Grid
{
    public class GridHtml
    {
        public GridHtml()
        {
            Child = new Dictionary<string, string>();
            HtmlAttributes = new Dictionary<string, string>();
        }

        internal string TagType { get; set; }
        internal string Name { get; set; }
        internal string Value { get; set; }
        internal bool UsePropertyValue { get; set; }
        internal IDictionary<string, string> Child { get; set; }
        internal IDictionary<string, string> HtmlAttributes { get; set; }
        internal bool Validation { get; set; }

        internal int ColSize { get; set; }

    }
}