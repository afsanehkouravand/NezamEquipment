namespace MyCommon.Helpers.ToolBox
{
    public static class ToolBoxButtonExtension
    {
        public static ToolBoxButton SetClass(this ToolBoxButton data, string @class)
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
        public static ToolBoxButton SetDisbaled(this ToolBoxButton data, bool isDisabled)
        {
            data.IsDisabled = isDisabled;
            return data;
        }
        public static ToolBoxButton SetStyle(this ToolBoxButton data, string style)
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
        public static ToolBoxButton SetAttribute(this ToolBoxButton data, string key, string value)
        {
            if (!data.HtmlAttributesForLink.ContainsKey(key))
            {
                data.HtmlAttributesForLink.Add(key, value);
            }
            else
            {
                data.HtmlAttributesForLink[key] = value;
            }

            return data;
        }
        public static ToolBoxButton SetQueryString(this ToolBoxButton data, string key, string value)
        {
            data.QureyStrings.Add(key, value);
            return data;
        }
        public static ToolBoxButton SetQueryString(this ToolBoxButton data, string key, object value)
        {
            data.QureyStrings.Add(key, value?.ToString() ?? "");
            return data;
        }
        public static ToolBoxButton SetIconCalss(this ToolBoxButton data, string iconClass)
        {
            data.IconClass = iconClass;
            return data;
        }
        public static ToolBoxButton SetGoToPage(this ToolBoxButton data, string url)
        {
            data.GoToPage = url;
            return data;
        }
        public static ToolBoxButton SetText(this ToolBoxButton data, string text)
        {
            data.Text = text;
            return data;
        }
        public static ToolBoxButton SetAjaxData(this ToolBoxButton data, string ajaxBegin = "OnBegin", 
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
        public static ToolBoxButton PopupText(this ToolBoxButton data, string text)
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
        public static ToolBoxButton SetExtraFunc(this ToolBoxButton data, string funcName)
        {
            if (!data.HtmlAttributesForLink.ContainsKey("extrafunc"))
            {
                data.HtmlAttributesForLink.Add("extrafunc", funcName);
            }
            else
            {
                data.HtmlAttributesForLink["extrafunc"] = funcName;
            }

            return data;
        }

    }
}