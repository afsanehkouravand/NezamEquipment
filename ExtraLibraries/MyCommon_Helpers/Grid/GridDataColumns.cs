using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Grid
{
    public class GridDataColumns
    {
        private readonly GridOption _option;
        private readonly HtmlHelper _htmlHelper;

        public GridDataColumns(GridOption option, HtmlHelper htmlHelper)
        {
            _option = option;
            _htmlHelper = htmlHelper;
        }

        public GridColumn AddColumnCounter()
        {
            var column = new GridColumn()
            {
                IsColumnCounter = true,
            };

            _option.GridColumns.Add(column);

            return column;
        }

        public GridColumn AddColumnForButton()
        {
            var column = new GridColumn()
            {
                IsColumnForButton = true,
            };

            _option.GridColumns.Add(column);

            return column;
        }

        public GridColumn AddColumnFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var propertyName = expression.GetPropertyName();

            var column = new GridColumn()
            {
                PropertyName = propertyName,
            };

            var propertyInfo = model.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                column.PropertyInfo = propertyInfo;
                var label = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (label != null)
                {
                    column.Label = label.Name;
                }
            }

            _option.GridColumns.Add(column);

            return column;
        }

        public GridHtml AddHtmlCheckBoxFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, string label = null, string value = null)
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
                    {"type", "checkbox" }
                },
                Name = column.PropertyName,
                Value = value
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

            _option.GridColumns.Add(column);

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlTextBoxFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, string label = null)
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

            _option.GridColumns.Add(column);

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlTextBoxFor(string name, string label = null)
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
            };

            _option.GridColumns.Add(column);

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHiddenInput<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var column = new GridColumn
            {
                PropertyName = expression.GetPropertyName(),
                IsHiddenInput = true,
                IsHtml = true,
            };

            column.IsHtmlTemplate = new GridHtml()
            {
                TagType = "input",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"type", "hidden" }
                },
                Name = column.PropertyName,
                UsePropertyValue = true,
            };

            _option.GridColumns.Add(column);

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlSelectFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, string label = null)
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

            _option.GridColumns.Add(column);

            return column.IsHtmlTemplate;
        }

        public GridHtml AddHtmlSelectFor(string name, string label = null)
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
            };

            _option.GridColumns.Add(column);

            return column.IsHtmlTemplate;
        }

    }
}