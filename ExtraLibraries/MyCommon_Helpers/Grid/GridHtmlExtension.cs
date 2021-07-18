using System.Collections.Generic;
using MyCommon.Helpers.Enum;

namespace MyCommon.Helpers.Grid
{
    public static class GridHtmlExtension
    {
        public static GridHtml IsCheckbox(this GridHtml data)
        {
            data.TagType = "input";

            if (!data.HtmlAttributes.ContainsKey("type"))
            {
                data.HtmlAttributes.Add("type", "checkbox");
            }
            else
            {
                data.HtmlAttributes["type"] = "checkbox";
            }

            return data;
        }

        public static GridHtml SetName(this GridHtml data, string name)
        {
            data.Name = name;
            return data;
        }
        public static GridHtml SetClass(this GridHtml data, string @class)
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
        public static GridHtml SetStyle(this GridHtml data, string style)
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
        public static GridHtml SetIsRequired(this GridHtml data, string message = null)
        {
            data.Validation = true;

            if (!data.HtmlAttributes.ContainsKey("data-val"))
            {
                data.HtmlAttributes.Add("data-val", "true");
            }

            if (!data.HtmlAttributes.ContainsKey("data-val-required"))
            {
                data.HtmlAttributes.Add("data-val-required", message ?? "پر کردن این فیلد ضروری است");
            }

            return data;
        }
        public static GridHtml SetAttribute(this GridHtml data, string key, string value)
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
        public static GridHtml SetUsePropertyValue(this GridHtml data, bool use)
        {
            data.UsePropertyValue = true;
            return data;
        }
        public static GridHtml SetChildTag(this GridHtml data, IDictionary<string, string> childTag)
        {
            data.Child = childTag;
            return data;
        }
        public static GridHtml IsDateFormat(this GridHtml data, DatePickerType datePickerType = DatePickerType.Shamsi,
            DatePickerPlacement datePickerPlacement = DatePickerPlacement.Bottom, string validationMessage = null)
        {
            if (!data.HtmlAttributes.ContainsKey("data-mddatetimepicker"))
            {
                data.HtmlAttributes.Add("data-mddatetimepicker", "true");
            }
            else
            {
                data.HtmlAttributes["data-mddatetimepicker"] = "true";
            }

            if (!data.HtmlAttributes.ContainsKey("data-trigger"))
            {
                data.HtmlAttributes.Add("data-trigger", "click");
            }
            else
            {
                data.HtmlAttributes["data-trigger"] = "click";
            }

            if (!data.HtmlAttributes.ContainsKey("data-targetselector"))
            {
                data.HtmlAttributes.Add("data-targetselector", "#" + data.Name);
            }
            else
            {
                data.HtmlAttributes["data-targetselector"] = "#" + data.Name;
            }

            var isgregorian = "false";
            var mdformat = "yyyy/MM/dd";
            switch (datePickerType)
            {
                case DatePickerType.Miladi:
                    isgregorian = "true";
                    mdformat = "MM/dd/yyyy";
                    break;
                case DatePickerType.Shamsi:
                    isgregorian = "false";
                    mdformat = "yyyy/MM/dd";
                    break;
            }

            if (!data.HtmlAttributes.ContainsKey("data-isgregorian"))
            {
                data.HtmlAttributes.Add("data-isgregorian", isgregorian);
            }
            else
            {
                data.HtmlAttributes["data-isgregorian"] = isgregorian;
            }

            if (!data.HtmlAttributes.ContainsKey("data-mdformat"))
            {
                data.HtmlAttributes.Add("data-mdformat", mdformat);
            }
            else
            {
                data.HtmlAttributes["data-mdformat"] = mdformat;
            }

            var placement = "bootom";
            switch (datePickerPlacement)
            {
                case DatePickerPlacement.Top:
                    placement = "top";
                    break;
                case DatePickerPlacement.Bottom:
                    placement = "bottom";
                    break;
                case DatePickerPlacement.Left:
                    placement = "left";
                    break;
                case DatePickerPlacement.Right:
                    placement = "right";
                    break;
            }

            if (!data.HtmlAttributes.ContainsKey("data-placement"))
            {
                data.HtmlAttributes.Add("data-placement", placement);
            }
            else
            {
                data.HtmlAttributes["data-placement"] = placement;
            }

            return data;
        }
    }
}