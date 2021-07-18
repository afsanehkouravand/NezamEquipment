using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Grid
{
    public static class GridButtonExtension
    {
        public static GridButton SetClass(this GridButton data, string @class)
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
        public static GridButton SetClass(this GridButton data, EGridButtonType type, string extralClass = null)
        {
            if (!data.HtmlAttributes.ContainsKey("class"))
            {
                data.HtmlAttributes.Add("class",
                    type.GetDisplayName() + (!string.IsNullOrWhiteSpace(extralClass) ? " " + extralClass : ""));
            }
            else
            {
                data.HtmlAttributes["class"] = type.GetDisplayName() +
                                               (!string.IsNullOrWhiteSpace(extralClass) ? " " + extralClass : "");
            }

            return data;
        }
        public static GridButton SetStyle(this GridButton data, string style)
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
        public static GridButton SetAttribute(this GridButton data, string key, string value)
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
        public static GridButton SetShowIf(this GridButton data, string propertyName, string value)
        {
            data.ShowIf = true;
            data.ShowIfPropertyName = propertyName;
            data.ShowIfValue = value;
            return data;
        }
        public static GridButton SetHideIf(this GridButton data, string propertyName, string value)
        {
            data.HideIf = true;
            data.HideIfPropertyName = propertyName;
            data.HideIfValue = value;
            return data;
        }
        public static GridButton SetQueryStringWithValue(this GridButton data, string key, string value)
        {
            data.QureyStrings.Add(new GridButtonQueryString()
            {
                Key = key,
                Value = value,
                IsWithValue = true,
            });
            return data;
        }
        public static GridButton SetQueryString(this GridButton data, string key, string propertyName)
        {
            data.QureyStrings.Add(new GridButtonQueryString()
            {
                Key = key,
                Value = propertyName,
            });
            return data;
        }
        public static GridButton SetQueryString(this GridButton data, string propertyName)
        {
            data.QureyStrings.Add(new GridButtonQueryString()
            {
                Key = nameof(propertyName),
                Value = propertyName,
            });
            return data;
        }
        public static GridButton SetIconCalss(this GridButton data, string iconClass)
        {
            data.IconClass = iconClass;
            return data;
        }
        public static GridButton SetGoToPage(this GridButton data, string url)
        {
            data.GoToPage = url;
            return data;
        }

        public static GridButton SetTagType(this GridButton data, string tag)
        {
            data.TagType = tag;
            return data;
        }

        public static GridButton SetText(this GridButton data, string text)
        {
            data.Text = text;
            return data;
        }

        public static GridButton SetAjaxData(this GridButton data, string ajaxBegin = "OnBegin", 
            string ajaxComplete = "OnComplete", string ajaxFailure = "OnFailure",
            string ajaxSuccess = "OnSuccess", string ajaxLoading = "#loading", string updateTarget = null)
        {
            if (!data.HtmlAttributes.ContainsKey("data-ajax"))
            {
                data.HtmlAttributes.Add("data-ajax", "true");
            }
            else
            {
                data.HtmlAttributes["data-ajax"] = "true";
            }

            if (!string.IsNullOrWhiteSpace(updateTarget))
            {
                if (!data.HtmlAttributes.ContainsKey("data-ajax-update"))
                {
                    data.HtmlAttributes.Add("data-ajax-update", $"#{updateTarget}");
                }
                else
                {
                    data.HtmlAttributes["data-ajax-update"] = $"#{updateTarget}";
                }
            }

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

            if (!data.HtmlAttributes.ContainsKey("data-ajax-url"))
            {
                data.HtmlAttributes.Add("data-ajax-url", "");
            }
            else
            {
                data.HtmlAttributes["data-ajax-url"] = "";
            }

            return data;
        }

        public static GridButton SetPopUpData(this GridButton data, string popupTitle = "")
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

        public static GridButton SetExtraFunc(this GridButton data, string funcName)
        {
            if (!data.HtmlAttributes.ContainsKey("extrafunc"))
            {
                data.HtmlAttributes.Add("extrafunc", funcName);
            }
            else
            {
                data.HtmlAttributes["extrafunc"] = funcName;
            }

            return data;
        }

    }
}