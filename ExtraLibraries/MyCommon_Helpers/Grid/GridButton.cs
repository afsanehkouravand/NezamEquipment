using System.Collections.Generic;

namespace MyCommon.Helpers.Grid
{
    public class GridButton
    {
        public GridButton()
        {
            HtmlAttributes = new Dictionary<string, string>();
            QureyStrings = new List<GridButtonQueryString>();
        }

        internal string Href { get; set; }

        internal string GoToPage { get; set; }

        internal string HaveAccessToArea { get; set; }

        internal string HaveAccessToController { get; set; }

        internal string HaveAccessToAction { get; set; }

        internal string Text { get; set; }

        internal string IconClass { get; set; }

        internal Dictionary<string, string> HtmlAttributes { get; set; }

        internal List<GridButtonQueryString> QureyStrings { get; set; }

        internal string TagType { get; set; }

        internal bool ShowIf { get; set; }

        internal string ShowIfPropertyName { get; set; }

        internal string ShowIfValue { get; set; }

        internal bool HideIf { get; set; }

        internal string HideIfPropertyName { get; set; }

        internal string HideIfValue { get; set; }
    }
}