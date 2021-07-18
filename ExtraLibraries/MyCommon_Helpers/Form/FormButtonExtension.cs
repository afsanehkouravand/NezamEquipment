namespace MyCommon.Helpers.Form
{
    public static class FormButtonExtension
    {
        public static FormButton SetClass(this FormButton data, string @class)
        {
            if (!data.HtmlAttributes.ContainsKey("class"))
            {
                data.HtmlAttributes.Add("class", @class);
            }
            else
            {
                data.HtmlAttributes["class"] = @class;
            }

            return data;
        }
        public static FormButton SetStyle(this FormButton data, string style)
        {
            if (!data.HtmlAttributes.ContainsKey("style"))
            {
                data.HtmlAttributes.Add("style", style);
            }
            else
            {
                data.HtmlAttributes["style"] = style;
            }

            return data;
        }
        public static FormButton SetAttribute(this FormButton data, string key, string value)
        {
            if (!data.HtmlAttributes.ContainsKey(key))
            {
                data.HtmlAttributes.Add(key, value);
            }
            else
            {
                data.HtmlAttributes[key] = value;
            }

            return data;
        }
        public static FormButton SetQueryString(this FormButton data, string key, string propertyName)
        {
            data.QureyStrings.Add(key, propertyName);
            return data;
        }
        public static FormButton SetIconCalss(this FormButton data, string iconClass)
        {
            data.IconClass = iconClass;
            return data;
        }
        public static FormButton SetText(this FormButton data, string text)
        {
            data.Text = text;
            return data;
        }
        public static FormButton PopupText(this FormButton data, string text)
        {
            if (!data.HtmlAttributes.ContainsKey("data-title"))
            {
                data.HtmlAttributes.Add("data-title", text);
            }
            else
            {
                data.HtmlAttributes["data-title"] = text;
            }

            return data;
        }

    }
}