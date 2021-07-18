using System.Collections.Generic;

namespace MyCommon.Helpers.Form
{
    public class FormButton
    {
        public FormButton()
        {
            HtmlAttributes = new Dictionary<string, string>();
            QureyStrings = new Dictionary<string, string>();
        }

        internal string Href { get; set; }

        internal string HaveAccessToArea { get; set; }

        internal string HaveAccessToController { get; set; }

        internal string HaveAccessToAction { get; set; }

        internal string Text { get; set; }

        internal string IconClass { get; set; }

        internal Dictionary<string, string> HtmlAttributes { get; set; }

        internal Dictionary<string, string> QureyStrings { get; set; }

    }
}