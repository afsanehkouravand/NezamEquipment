using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MyCommon.Helpers.Enum;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Helper
{
    public static class DatePickerForHelper
    {
        public static MvcHtmlString DatePickerFor<TModel, TProperty>
        (this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null, DatePickerType type = DatePickerType.Shamsi,
            DatePickerPlacement placement = DatePickerPlacement.Top, string @class = "")
        {
            var datePickerName = ExpressionHelper.GetExpressionText(expression);
            var datePickerFullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(datePickerName);
            var datePickerId = TagBuilder.CreateSanitizedId(datePickerFullName);

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var tag = new TagBuilder("input");
            tag.Attributes.Add("name", datePickerFullName);
            tag.Attributes.Add("id", datePickerId);
            tag.Attributes.Add("class", "form-control" + (string.IsNullOrWhiteSpace(@class) ? $" {@class}" : ""));
            tag.Attributes.Add("data-mddatetimepicker", "true");
            tag.Attributes.Add("data-trigger", "click");
            tag.Attributes.Add("data-targetselector", "#"+datePickerId);
            tag.Attributes.Add("type", "text");

            if (htmlAttributes != null)
            {
                var properties = htmlAttributes.GetType().GetProperties();
                if (properties.Any())
                {
                    foreach (var propertyInfo in properties)
                    {
                        if (tag.Attributes.ContainsKey(propertyInfo.Name))
                        {
                            tag.Attributes.Remove(propertyInfo.Name);
                            var value = propertyInfo.GetValue(htmlAttributes, null);
                            tag.Attributes.Add(propertyInfo.Name, value.ToString());
                        }
                    }
                }
            }

            switch (type)
            {
                case DatePickerType.Miladi:
                    tag.Attributes.Add("data-isgregorian", "true");
                    tag.Attributes.Add("data-mdformat", "MM/dd/yyyy");

                    break;
                case DatePickerType.Shamsi:
                    tag.Attributes.Add("data-isgregorian", "false");
                    tag.Attributes.Add("data-mdformat", "yyyy/MM/dd");
                    break;
                default:
                    tag.Attributes.Add("data-isgregorian", "false");
                    tag.Attributes.Add("data-mdformat", "yyyy/MM/dd");
                    break;
            }


            var datePickerValue = string.Empty;
            //if (metadata.Model != null && (metadata.Model.ToString() != "1/1/0001 12:00:00 AM" && metadata.Model.ToString() != "01/01/0001 12:00:00 ق.ظ"))
            if (metadata.Model != null && DateTime.Parse(metadata.Model.ToString()).Year != 0001)
            {
                if (type == DatePickerType.Miladi)
                {
                    datePickerValue = DateTime.Parse(metadata.Model.ToString()).ToString("MM/dd/yyyy");
                } else if (type == DatePickerType.Shamsi)
                {
                    datePickerValue = DateTime.Parse(metadata.Model.ToString()).ToShortShamsi();
                }
            }
            tag.Attributes.Add("value", datePickerValue);

            switch (placement)
            {
                case DatePickerPlacement.Top:
                    tag.Attributes.Add("data-placement", "top");
                    break;
                case DatePickerPlacement.Bottom:
                    tag.Attributes.Add("data-placement", "bottom");
                    break;
                case DatePickerPlacement.Left:
                    tag.Attributes.Add("data-placement", "left");
                    break;
                case DatePickerPlacement.Right:
                    tag.Attributes.Add("data-placement", "right");
                    break;
                default:
                    tag.Attributes.Add("data-placement", "top");
                    break;
            }

            var validationAttributes = helper.GetUnobtrusiveValidationAttributes(datePickerFullName, metadata);

            foreach (string key in validationAttributes.Keys)
            {
                switch (key)
                {
                    case "data-val-date":
                        //tag.Attributes.Add(key, "تاریخ وارد شده صحیح نمی باشد");
                        break;
                    case "data-val-required":
                        tag.Attributes.Add(key, $"{metadata.DisplayName} ضروری است.");
                        break;
                    default:
                        tag.Attributes.Add(key, validationAttributes[key].ToString());
                        break;
                }
            }

            var html = new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
            return html;
        }


    }
}