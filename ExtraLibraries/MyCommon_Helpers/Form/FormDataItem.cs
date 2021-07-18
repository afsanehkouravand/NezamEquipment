using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Form
{
    public class FormDataItem
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly FormOption _option;

        public FormDataItem(FormOption options, HtmlHelper htmlHelper)
        {
            _option = options;
            _htmlHelper = htmlHelper;
        }

        public void AddTitle(string text)
        {
            var item = new FormItem()
            {
                Tag = "legend",
                Class = "margin-bottom-10",
                ColSize = 12,
                Text = text,
            };

            _option.FormItems.Add(item);
        }

        public void AddLine()
        {
            var item = new FormItem()
            {
                Tag = "hr",
                ColSize = 12
            };

            _option.FormItems.Add(item);
        }

        public FormItem TextBox(string fieldname, int colSize, string value = "")
        {
            var item = new FormItem
            {
                Tag = "input",
                Type = "text",
                Class = "form-control",
                Name = fieldname,
                ColSize = colSize,
                Value = value,
            };

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem Captcha(string fieldname, int colSize, string captchaname = "Captcha")
        {
            var item = new FormItem
            {
                Tag = "input",
                Type = "text",
                Class = "form-control",
                Name = fieldname,
                ColSize = colSize,
                Value = "",
                IsCaptcha = true,
                CaptchaName = captchaname,
            };

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem TextBox<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, int colSize, bool isltr = false)
        {
            var item = new FormItem
            {
                Tag = "input",
                Type = "text",
                Class = "form-control" + (isltr ? " ltr text-right" : ""),
                Name = expression.GetPropertyName(),
                ColSize = colSize,
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem NumberBox<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, int colSize)
        {
            var item = new FormItem
            {
                Tag = "input",
                Type = "number",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem PassBox<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, int colSize)
        {
            var item = new FormItem
            {
                Tag = "input",
                Type = "password",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem AreaBox<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, int colSize)
        {
            var item = new FormItem
            {
                Tag = "textarea",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem Boolean<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, 
            int colSize, string onTrue = "فعال", string onFalse = "غیر فعال")
        {
            var item = new FormItem
            {
                Tag = "select",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
                Child = new Dictionary<string, string>()
                {
                    {"","انتخاب کنید" },
                    {true.ToString(), onTrue},
                    {false.ToString(), onFalse},
                }
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem Boolean(string name, int colSize, string onTrue = "فعال", string onFalse = "غیر فعال", bool? value = null)
        {
            var item = new FormItem
            {
                Tag = "select",
                Class = "form-control",
                Name = name,
                ColSize = colSize,
                Child = new Dictionary<string, string>()
                {
                    {true.ToString(), onTrue},
                    {false.ToString(), onFalse},
                },
                Value = value?.ToString() ?? ""
            };

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem EnumDropDown<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            int colSize)
        {
            var item = new FormItem
            {
                Tag = "select",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = ((int)value).ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }

                item.Child = new Dictionary<string, string>()
                {
                    {"", "انتخاب کنید"}
                };

                var myDics = propertyInfo.AnonymousEnumToDictionary();
                foreach (var dic in myDics)
                {
                    item.Child.Add(dic.Key.ToString(), dic.Value);
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem DropDown<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            int colSize, IDictionary<string, string> childOptions, bool useChooseOption = true)
        {
            var item = new FormItem
            {
                Tag = "select",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
                Child = childOptions,
                UseChooseOption = useChooseOption,
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }

            }

            _option.FormItems.Add(item);

            return item;
        }

        public FormItem DropDown<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            int colSize, IDictionary<string, IDictionary<string, string>> childOptions, bool useChooseOption = true)
        {
            var item = new FormItem
            {
                Tag = "select",
                Class = "form-control",
                Name = expression.GetPropertyName(),
                ColSize = colSize,
                ChildGroup = childOptions,
                UseChooseOption = useChooseOption
            };

            var propertyInfo = model.GetType().GetProperty(item.Name);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value.ToString();
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.FormItems.Add(item);

            return item;
        }

    }
}