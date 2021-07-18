using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Grid
{
    public class GridDataFooter
    {
        private readonly GridOption _option;
        private readonly HtmlHelper _htmlHelper;

        public GridDataFooter(GridOption option, HtmlHelper htmlHelper)
        {
            _option = option;
            _htmlHelper = htmlHelper;
        }

        public GridHtml AddHtmlTextBoxFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int colSize, string label = null)
        {
            var propertyName = expression.GetPropertyName();

            var column = new GridColumn()
            {
                PropertyName = propertyName,
                IsHtml = true,
            };

            column.IsHtmlTemplate = new GridHtml()
            {
                TagType = "input",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"type", "text" }
                },
                Name = column.PropertyName,
                UsePropertyValue = true,
                ColSize = colSize,
            };

            if (!string.IsNullOrWhiteSpace(label))
            {
                column.Label = label;
            }
            else
            {
                var propertyInfo = model.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    column.PropertyInfo = propertyInfo;
                    var labelProperty = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                    if (labelProperty != null)
                    {
                        column.Label = labelProperty.Name;
                    }
                }
            }

            _option.GridFooter.Add(column);
            _option.IsSetTableFooter = true;

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlTextBoxFor(string name, int colSize, string label = null)
        {
            var column = new GridColumn
            {
                PropertyName = name,
                IsHtml = true,
                Label = label,
            };

            column.IsHtmlTemplate = new GridHtml()
            {
                TagType = "input",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"type", "text" }
                },
                Name = column.PropertyName,
                ColSize = colSize,
            };

            _option.GridFooter.Add(column);
            _option.IsSetTableFooter = true;

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlSelectFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int colSize, string label = null, string value = "")
        {
            var propertyName = expression.GetPropertyName();

            var column = new GridColumn()
            {
                PropertyName = propertyName,
                IsHtml = true,
            };

            column.IsHtmlTemplate = new GridHtml()
            {
                TagType = "select",
                Name = column.PropertyName,
                UsePropertyValue = true,
                ColSize = colSize,
                Value = value,
            };

            if (!string.IsNullOrWhiteSpace(label))
            {
                column.Label = label;
            }
            else
            {
                var propertyInfo = model.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    column.PropertyInfo = propertyInfo;
                    var labelProperty = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                    if (labelProperty != null)
                    {
                        column.Label = labelProperty.Name;
                    }
                }
            }

            _option.GridFooter.Add(column);
            _option.IsSetTableFooter = true;

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlSelectFor(string name, int colSize, string label = null)
        {
            var column = new GridColumn
            {
                PropertyName = name,
                IsHtml = true,
                Label = label,
            };

            column.IsHtmlTemplate = new GridHtml()
            {
                TagType = "select",
                Name = column.PropertyName,
                ColSize = colSize,
            };

            _option.GridFooter.Add(column);
            _option.IsSetTableFooter = true;

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlHiddenInputFor(string key, string value)
        {
            var column = new GridColumn
            {
                PropertyName = key,
                IsHtml = true,
                Label = string.Empty,
            };

            column.IsHtmlTemplate = new GridHtml()
            {
                TagType = "input",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"type", "hidden" },
                    {"value", value },
                },
                Name = column.PropertyName,
                ColSize = 0,
            };

            _option.GridFooter.Add(column);
            _option.IsSetTableFooter = true;

            return column.IsHtmlTemplate;
        }

        public GridButton AddButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(string url)
        {
            var info = new GetUrlInfoExtension.GetUrlInfo()
            {
                Area = string.Empty,
                Controller = string.Empty,
                Action = string.Empty,
                Url = url,
            };
            return GetButton(info);
        }
        private GridButton GetButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
                TagType = "a",
            };

            _option.FooterButtons.Add(button);
            _option.IsSetTableFooter = true;

            return button;
        }

        public GridButton AddAjaxButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public GridButton AddAjaxButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public GridButton AddAjaxButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public GridButton AddAjaxButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        private GridButton GetAjaxButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                TagType = "a",
                Href = info.Url,
            };

            button.HtmlAttributes.Add("data-ajax", "true");
            button.HtmlAttributes.Add("data-ajax-method", "post");
            button.HtmlAttributes.Add("data-ajax-loading", "#loading");
            button.HtmlAttributes.Add("data-ajax-complete", "OnComplete");
            button.HtmlAttributes.Add("data-ajax-begin", "OnBegin");
            button.HtmlAttributes.Add("data-ajax-failure", "OnFailure");
            button.HtmlAttributes.Add("data-ajax-success", "OnSuccess");
            button.HtmlAttributes.Add("data-ajax-url", info.Url);

            _option.FooterButtons.Add(button);
            _option.IsSetTableFooter = true;

            return button;
        }

    }
}