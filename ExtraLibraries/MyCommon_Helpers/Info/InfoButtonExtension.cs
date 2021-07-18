namespace MyCommon.Helpers.Info
{
    public static class InfoButtonExtension
    {
        public static InfoButton SetClass(this InfoButton data, string @class)
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
        public static InfoButton SetStyle(this InfoButton data, string style)
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
        public static InfoButton SetAttribute(this InfoButton data, string key, string value)
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
        public static InfoButton SetQueryString(this InfoButton data, string key, string propertyName)
        {
            data.QureyStrings.Add(key, propertyName);
            return data;
        }
        public static InfoButton SetIconCalss(this InfoButton data, string iconClass)
        {
            data.IconClass = iconClass;
            return data;
        }
        public static InfoButton SetGoToPage(this InfoButton data, string url)
        {
            data.GoToPage = url;
            return data;
        }
        public static InfoButton SetText(this InfoButton data, string text)
        {
            data.Text = text;
            return data;
        }
        public static InfoButton SetAjaxData(this InfoButton data, string ajaxBegin = "OnBegin",
            string ajaxComplete = "OnComplete", string ajaxFailure = "OnFailure",
            string ajaxSuccess = "OnSuccess", string ajaxLoading = "#loading")
        {
            if (!data.HtmlAttributes.ContainsKey("data-ajax-begin"))
            {
                data.HtmlAttributes.Add("data-ajax-begin", ajaxBegin);
            }
            else
            {
                data.HtmlAttributes["data-ajax-begin"] = ajaxBegin;
            }

            if (!data.HtmlAttributes.ContainsKey("data-ajax-complete"))
            {
                data.HtmlAttributes.Add("data-ajax-complete", ajaxComplete);
            }
            else
            {
                data.HtmlAttributes["data-ajax-complete"] = ajaxComplete;
            }

            if (!data.HtmlAttributes.ContainsKey("data-ajax-failure"))
            {
                data.HtmlAttributes.Add("data-ajax-failure", ajaxFailure);
            }
            else
            {
                data.HtmlAttributes["data-ajax-failure"] = ajaxFailure;
            }

            if (!data.HtmlAttributes.ContainsKey("data-ajax-success"))
            {
                data.HtmlAttributes.Add("data-ajax-success", ajaxSuccess);
            }
            else
            {
                data.HtmlAttributes["data-ajax-success"] = ajaxSuccess;
            }

            if (!data.HtmlAttributes.ContainsKey("data-ajax-loading"))
            {
                data.HtmlAttributes.Add("data-ajax-loading", ajaxLoading);
            }
            else
            {
                data.HtmlAttributes["data-ajax-loading"] = ajaxLoading;
            }

            return data;
        }
        public static InfoButton SetPopUpData(this InfoButton data, string popupTitle = "")
        {
            if (!data.HtmlAttributes.ContainsKey("data-title"))
            {
                data.HtmlAttributes.Add("data-title", popupTitle);
            }
            else
            {
                data.HtmlAttributes["data-title"] = popupTitle;
            }

            if (!data.HtmlAttributes.ContainsKey("data-popup"))
            {
                data.HtmlAttributes.Add("data-popup", "true");
            }
            else
            {
                data.HtmlAttributes["data-popup"] = "true";
            }

            if (!data.HtmlAttributes.ContainsKey("data-url"))
            {
                data.HtmlAttributes.Add("data-url", "");
            }
            else
            {
                data.HtmlAttributes["data-url"] = "";
            }

            return data;
        }

    }
}