using System.Collections.Generic;
using MyCommon.Helpers.Enum;

namespace MyCommon.Helpers.Form
{
    public static class FormItemExtension
    {
        public static FormItem Class(this FormItem data, string @class)
        {
            data.Class = @class;
            return data;
        }

        public static FormItem Style(this FormItem data, string style)
        {
            data.Style = style;
            return data;
        }

        public static FormItem SetAttribute(this FormItem data, string key, string value)
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

        public static FormItem Label(this FormItem data, string label)
        {
            data.Label = label;
            return data;
        }

        public static FormItem Value(this FormItem data, string value)
        {
            data.Value = value;
            return data;
        }

        public static FormItem IsRequired(this FormItem data, string message = null)
        {
            data.DataVal = true;
            data.DataValRequired = message ?? "پر کردن این فیلد ضروری است";
            return data;
        }

        public static FormItem IsMobileNumber(this FormItem data, string message = null)
        {
            data.DataVal = true;

            if (!data.HtmlAttributes.ContainsKey("data-val-regex-pattern"))
            {
                data.HtmlAttributes.Add("data-val-regex-pattern", "(\\+98|0098|0)?9\\d{9}");
            }

            if (!data.HtmlAttributes.ContainsKey("data-val-regex"))
            {
                data.HtmlAttributes.Add("data-val-regex", message ?? "شماره تلفن همراه صحیح نمی باشد.");
            }
            else
            {
                data.HtmlAttributes["data-val-regex"] = message ?? "شماره تلفن همراه صحیح نمی باشد.";
            }

            return data;
        }

        public static FormItem IsEmailAddress(this FormItem data, string message = null)
        {
            data.DataVal = true;

            if (!data.HtmlAttributes.ContainsKey("data-val-email"))
            {
                data.HtmlAttributes.Add("data-val-email", message ?? "ایمیل وارد شده صحیح نمی باشد.");
            }
            else
            {
                data.HtmlAttributes["data-val-email"] = message ?? "ایمیل وارد شده صحیح نمی باشد.";
            }

            return data;
        }

        public static FormItem IsDateFormat(this FormItem data, DatePickerType datePickerType = DatePickerType.Shamsi,
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

        public static FormItem SetDataList(this FormItem data, IList<string> datalist)
        {
            data.DataList = datalist;
            return data;
        }

    }
}