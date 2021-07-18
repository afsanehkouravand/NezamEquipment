using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Info
{
    public class InfoDataItem
    {
        private readonly  InfoOption _option;

        public InfoDataItem(InfoOption options)
        {
            _option = options;
        }

        public InfoItem AddFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int? colSize = null)
        {
            var item = new InfoItem()
            {
                PropertyName = expression.GetPropertyName(),
                ColSize = colSize,
            };

            var propertyInfo = model.GetType().GetProperty(item.PropertyName);
            if (propertyInfo != null)
            {
                item.PropertyInfo = propertyInfo;

                var value = propertyInfo.GetValue(model, null);
                if (value != null)
                {
                    item.Value = value;
                }

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    item.Label = displayAttribute.Name;
                }
            }

            _option.InfoItems.Add(item);

            return item;
        }

        public InfoItem AddFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression, 
            Expression<Func<TModel, TProperty>> expression2, string separator = null, int? colSize = null)
        {
            var tochar = Convert.ToChar(".");

            var ename1 = expression.GetPropertyName();
            if (ename1.Contains(tochar))
            {
                ename1 = ename1.Split(tochar)[0];
            }

            var ename2 = expression2.GetPropertyName();
            if (ename2.Contains(tochar))
            {
                ename2 = ename2.Split(tochar)[0];
            }

            var item = new InfoItem()
            {
                PropertyName = ename1 + " " + ename2,
                ColSize = colSize,
            };

            var listLabel = new List<string>();
            var listValue = new List<string>();

            var propertyInfo = model.GetType().GetProperty(ename1);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(model, null);
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                    listValue.Add(value.ToString());

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    listLabel.Add(displayAttribute.Name);
                }
            }

            var propertyInfo2 = model.GetType().GetProperty(ename2);
            if (propertyInfo2 != null)
            {
                var value = propertyInfo2.GetValue(model, null);
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                    listValue.Add(value.ToString());

                var displayAttribute = propertyInfo2.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    listLabel.Add(displayAttribute.Name);
                }
            }

            if (listLabel.Any())
                item.Label = string.Join(" و ", listLabel);

            if (listValue.Any())
                item.Value = string.Join(!string.IsNullOrWhiteSpace(separator) ? $" {separator} " : " ", listValue);

            _option.InfoItems.Add(item);

            return item;
        }

        public InfoItem AddFor<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            Expression<Func<TModel, TProperty>> expression2, Expression<Func<TModel, TProperty>> expression3,
            string separator = null, int? colSize = null)
        {
            var tochar = Convert.ToChar(".");

            var ename1 = expression.GetPropertyName();
            if (ename1.Contains(tochar))
            {
                ename1 = ename1.Split(tochar)[0];
            }

            var ename2 = expression2.GetPropertyName();
            if (ename2.Contains(tochar))
            {
                ename2 = ename2.Split(tochar)[0];
            }

            var ename3 = expression3.GetPropertyName();
            if (ename3.Contains(tochar))
            {
                ename3 = ename3.Split(tochar)[0];
            }

            var item = new InfoItem()
            {
                PropertyName = ename1 + " " + ename2 + " " + ename3,
                ColSize = colSize,
            };

            var listLabel = new List<string>();
            var listValue = new List<string>();

            var propertyInfo = model.GetType().GetProperty(ename1);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(model, null);
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                    listValue.Add(value.ToString());

                var displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    listLabel.Add(displayAttribute.Name);
                }
            }

            var propertyInfo2 = model.GetType().GetProperty(ename2);
            if (propertyInfo2 != null)
            {
                var value = propertyInfo2.GetValue(model, null);
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                    listValue.Add(value.ToString());

                var displayAttribute = propertyInfo2.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    listLabel.Add(displayAttribute.Name);
                }
            }

            var propertyInfo3 = model.GetType().GetProperty(ename3);
            if (propertyInfo3 != null)
            {
                var value = propertyInfo3.GetValue(model, null);
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                    listValue.Add(value.ToString());

                var displayAttribute = propertyInfo3.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    listLabel.Add(displayAttribute.Name);
                }
            }

            if (listLabel.Any())
                item.Label = string.Join(!string.IsNullOrWhiteSpace(separator) ? $" {separator} " : " و ", listLabel);
            
            if (listValue.Any())
                item.Value = string.Join(!string.IsNullOrWhiteSpace(separator) ? $" {separator} " : " ", listValue);

            _option.InfoItems.Add(item);

            return item;
        }

        public InfoItem AddFor(string text, int? colSize = null)
        {
            var item = new InfoItem()
            {
                Value = text,
                ColSize = colSize,
            };

            _option.InfoItems.Add(item);

            return item;
        }

        public InfoItem AddEmptyLine()
        {
            var item = new InfoItem()
            {
                Value = string.Empty,
                ColSize = 12,
                IsEmptyLine = true,
            };

            _option.InfoItems.Add(item);

            return item;
        }

        public InfoItem AddTitle(string title)
        {
            var item = new InfoItem()
            {
                Value = title,
                ColSize = 12,
                IsTitle = true,
            };

            _option.InfoItems.Add(item);

            return item;
        }


    }
}