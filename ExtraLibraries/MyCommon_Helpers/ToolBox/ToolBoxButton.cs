using System.Collections.Generic;

namespace MyCommon.Helpers.ToolBox
{
    public class ToolBoxButton
    {
        public ToolBoxButton()
        {
            HtmlAttributes = new Dictionary<string, string>();
            HtmlAttributesForLink = new Dictionary<string, string>();
            QureyStrings = new Dictionary<string, string>();
        }

        internal string Href { get; set; }

        internal string GoToPage { get; set; }

        internal string HaveAccessToArea { get; set; }

        internal string HaveAccessToController { get; set; }

        internal string HaveAccessToAction { get; set; }

        internal string Text { get; set; }

        internal string IconClass { get; set; }

        internal Dictionary<string, string> HtmlAttributes { get; set; }
        internal Dictionary<string, string> HtmlAttributesForLink { get; set; }

        internal Dictionary<string, string> QureyStrings { get; set; }

        internal bool IsDisabled { get; set; }

        internal bool IsAjaxForm { get; set; }
    }
}