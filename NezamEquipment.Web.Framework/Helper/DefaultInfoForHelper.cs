using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class DefaultInFoForHelper
    {
        /// <summary>
        /// نمایش اطلاعات در جدول
        /// </summary>
        /// <typeparam name="TModel">مدل اطلاعات</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="title">عنوان جدول</param>
        /// <param name="colume">تعداد ستون های جدول</param>
        /// <param name="color">رنگ جدول</param>
        /// <returns></returns>
        public static DefaultInfoForExtension DefaultInfoFor<TModel>
            (this HtmlHelper<TModel> htmlHelper, string title = "",
            DefaultInfoForColumeType colume = DefaultInfoForColumeType.Column3,
            DefaultInfoForColorType color = DefaultInfoForColorType.Info)
        {
            return new DefaultInfoForExtension(title: title, colume: colume, color: color);
        }
    }

    public class DefaultInfoForExtension
    {
        protected DefaultInfoForOptions Options;

        public DefaultInfoForExtension(string title, DefaultInfoForColumeType colume, DefaultInfoForColorType color)
        {
            Options = new DefaultInfoForOptions
            {
                Title = title,
                Colume = colume,
                Color = color,
            };
        }

        public DefaultInfoForExtension Items(Action<DefaultInfoForData> buildOptions)
        {
            buildOptions(new DefaultInfoForData(options: Options));
            return this;
        }

        public MvcHtmlString Done()
        {
            var divPanel = new TagBuilder("div") {InnerHtml = ""};
            divPanel.AddCssClass($"panel panel-{Options.Color.GetDisplayName()} defaultInfo");

            var divPanelHeading = new TagBuilder("div");
            divPanelHeading.AddCssClass("panel-heading");

            var divPanelHeadingH5 = new TagBuilder("h5");
            divPanelHeadingH5.AddCssClass("margin-0");
            divPanelHeadingH5.SetInnerText(Options.Title);

            divPanelHeading.InnerHtml = divPanelHeadingH5.ToString();

            divPanel.InnerHtml += divPanelHeading.ToString();

            var divTableResponsive = new TagBuilder("div");
            divTableResponsive.AddCssClass("table-responsive");

            var table = new TagBuilder("table");
            table.AddCssClass("table");

            var tbody = new TagBuilder("tbody");

            foreach (var a in Options.DefaultInfoForItems)
            {
                if (a.ColSize != null)
                    continue;
                
                if (Options.Colume == DefaultInfoForColumeType.Column6)
                {
                    a.ColSize = (int)DefaultInfoForColumeType.Column6;
                }
                else if (Options.Colume == DefaultInfoForColumeType.Column4)
                {
                    a.ColSize = (int)DefaultInfoForColumeType.Column4;
                }
                else if (Options.Colume == DefaultInfoForColumeType.Column3)
                {
                    a.ColSize = (int) DefaultInfoForColumeType.Column3;
                }
                else if (Options.Colume == DefaultInfoForColumeType.Column2)
                {
                    a.ColSize = (int) DefaultInfoForColumeType.Column2;
                }
                else if (Options.Colume == DefaultInfoForColumeType.Column1)
                {
                    a.ColSize = (int) DefaultInfoForColumeType.Column1;
                }
            }

            var listOfDefaultInfoForItem = new List<List<DefaultInfoForItem>>();
            var secondList = new List<DefaultInfoForItem>();
            var colume = 0;
            foreach (var item in Options.DefaultInfoForItems)
            {
                colume += item.ColSize ?? 0;
                if (colume == 12)
                {
                    secondList.Add(item);
                    listOfDefaultInfoForItem.Add(secondList.Clone());
                    secondList.Clear();
                    colume = 0;
                }
                else if (colume < 12)
                {
                    secondList.Add(item);
                }
                else if (colume > 12)
                {
                    listOfDefaultInfoForItem.Add(secondList.Clone());
                    secondList.Clear();
                    secondList.Add(item);
                    colume = item.ColSize ?? 0;
                }
            }

            if (secondList.Any())
            {
                listOfDefaultInfoForItem.Add(secondList);
            }

            foreach (var item in listOfDefaultInfoForItem)
            {
                if (Options.Colume == DefaultInfoForColumeType.Column2)
                {
                    if (item.Count != 2)
                    {
                        item.Last().Colspan = 2 - item.Count + 1;
                        continue;
                    }
                }
                if (Options.Colume == DefaultInfoForColumeType.Column3)
                {
                    if (item.Count != 3)
                    {
                        item.Last().Colspan = 3 - item.Count + 1;
                        continue;
                    }
                }
                if (Options.Colume == DefaultInfoForColumeType.Column4)
                {
                    if (item.Count != 4)
                    {
                        item.Last().Colspan = 4 - item.Count + 1;
                        continue;
                    }
                }
                if (Options.Colume == DefaultInfoForColumeType.Column6)
                {
                    if (item.Count != 6)
                    {
                        item.Last().Colspan = 6 - item.Count + 1;
                    }
                }
            }

            foreach (var rowItems in listOfDefaultInfoForItem)
            {
                var tr = new TagBuilder("tr"){InnerHtml = ""};

                foreach (var item in rowItems)
                {
                    var td = new TagBuilder("td") { InnerHtml = "" };
                    if (item.Colspan != null)
                    {
                        td.MergeAttribute("colspan", item.Colspan.ToString());
                    }

                    var label = new TagBuilder("label");
                    label.AddCssClass("small");
                    label.SetInnerText(item.PropertyName);

                    var span = new TagBuilder("span");

                    var modelType = item.Model.GetType();

                    if (item.IsBoolean)
                    {
                        var propertyInfo = modelType.GetProperty(item.PropertyName);
                        if (propertyInfo != null)
                        {
                            var value = propertyInfo.GetValue(item.Model, null);
                            value = PropertValue(propertyInfo, value);
                            if (value != null)
                            {
                                span.InnerHtml = bool.Parse(value.ToString())
                                    ? item.BooleanOnTrueTemplate
                                    : item.BooleanOnFalseTemplate;
                            }
                            else
                            {
                                span.SetInnerText("-");
                            }

                            if (string.IsNullOrWhiteSpace(item.PropertyTitle))
                            {
                                var attar = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                                label.SetInnerText(attar != null ? attar.Name : item.PropertyName);
                            }
                            else
                            {
                                label.SetInnerText(item.PropertyTitle);
                            }
                        }

                        td.InnerHtml += $"{label}: {span}";
                    }

                    else if (item.IsList)
                    {
                        span.InnerHtml = string.Empty;

                        var propertyInfo = modelType.GetProperty(item.PropertyName);
                        if (propertyInfo != null)
                        {
                            var propertModel = propertyInfo.GetValue(item.Model, null);
                            if (propertModel != null)
                            {
                                foreach (var l in (IList)propertModel)
                                {
                                    var memberInfo = l.GetType().GetProperty(item.ListPropertyName);
                                    if (memberInfo != null)
                                    {
                                        var value = memberInfo.GetValue(l, null);
                                        value = PropertValue(memberInfo, value).ToString();
                                        if (!string.IsNullOrWhiteSpace(item.CustomTemplate))
                                        {
                                            span.InnerHtml += string.Format(item.CustomTemplate, value);
                                        }
                                        else
                                        {
                                            span.InnerHtml += $" {value} ";
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrWhiteSpace(item.PropertyTitle))
                            {
                                var attar = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                                label.SetInnerText(attar != null ? attar.Name : item.PropertyName);
                            }
                            else
                            {
                                label.SetInnerText(item.PropertyTitle);
                            }
                        }

                        if (string.IsNullOrWhiteSpace(span.InnerHtml))
                            span.InnerHtml = "-";

                        td.InnerHtml += $"{label}: {span}";
                    }

                    else if (item.IsMultiProperty)
                    {
                        span.InnerHtml = string.Empty;
                        var title = new List<string>();

                        foreach (var name in item.MultiPropertyName)
                        {
                            var propertyInfo = modelType.GetProperty(name);
                            if (propertyInfo != null)
                            {
                                var value = propertyInfo.GetValue(item.Model, null);
                                if (value != null)
                                {
                                    value = PropertValue(propertyInfo, value).ToString();
                                    if (!string.IsNullOrWhiteSpace(item.CustomTemplate))
                                    {
                                        span.InnerHtml += string.Format(item.CustomTemplate, value);
                                    }
                                    else
                                    {
                                        span.InnerHtml += $" {value} ";
                                    }
                                }

                                if (string.IsNullOrWhiteSpace(item.PropertyTitle))
                                {
                                    var attr = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                                    if (attr != null)
                                    {
                                        title.Add(attr.Name);
                                    }
                                }
                            }

                        }

                        if (string.IsNullOrWhiteSpace(item.PropertyTitle))
                        {
                            label.SetInnerText(title.Any()
                                ? string.Join(" و ", title)
                                : string.Join(" و ", item.MultiPropertyName));
                        }
                        else
                        {
                            label.SetInnerText(item.PropertyTitle);
                        }

                        if (string.IsNullOrWhiteSpace(span.InnerHtml))
                            span.InnerHtml += "-";

                        td.InnerHtml += $"{label}: {span}";
                    }

                    else
                    {
                        var propertyInfo = modelType.GetProperty(item.PropertyName);
                        if (propertyInfo != null)
                        {
                            var value = propertyInfo.GetValue(item.Model, null);
                            span.InnerHtml = value != null ? PropertValue(propertyInfo, value).ToString() : "-";

                            if (string.IsNullOrWhiteSpace(item.PropertyTitle))
                            {
                                var attar = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                                label.SetInnerText(attar != null ? attar.Name : item.PropertyName);
                            }
                            else
                            {
                                label.SetInnerText(item.PropertyTitle);
                            }
                        }
                        else
                        {
                            span.InnerHtml = "-";
                        }

                        td.InnerHtml += $"{label}: {span}";
                    }

                    tr.InnerHtml += td;
                }

                tbody.InnerHtml += tr.ToString();
            }

            table.InnerHtml = tbody.ToString();

            divTableResponsive.InnerHtml = table.ToString();

            divPanel.InnerHtml += divTableResponsive.ToString();

            if (Options.EnablePanelFooter && Options.PanelFooterButtons.Any())
            {
                var divPanelFooter = new TagBuilder("div");
                divPanelFooter.AddCssClass("panel-footer");

                if (Options.PanelFooterButtons.Any())
                {
                    foreach (var item in Options.PanelFooterButtons)
                    {
                        var haveAccessTo = true;
                        if (Options.SetAuthorize && Options.HaveAccessTo != null)
                        {
                            haveAccessTo = Options.HaveAccessTo.Check(item.HaveAccessToContrller, item.HaveAccessToAction);
                        }
                        if (haveAccessTo)
                        {
                            var tag = new TagBuilder("a");
                            if (item.HtmlAttributes != null)
                            {
                                foreach (var attribute in item.HtmlAttributes)
                                {
                                    tag.MergeAttribute(attribute.Key, attribute.Value);
                                }
                            }

                            tag.SetInnerText(item.Text);

                            divPanelFooter.InnerHtml += tag;
                        }
                    }
                }

                divPanel.InnerHtml += divPanelFooter.ToString();
            }

            var html = new MvcHtmlString(divPanel.ToString());
            return html;
        }

        private static object PropertValue(PropertyInfo firstOrDefault, object propertValue)
        {
            if (firstOrDefault.PropertyType.IsNullableEnum())
            {
                var answer = Nullable.GetUnderlyingType(firstOrDefault.PropertyType);
                if (answer != null)
                {
                    foreach (var fieldInfo in answer.GetFields())
                    {
                        if (fieldInfo.Name == propertValue.ToString())
                        {
                            var pv = fieldInfo.GetCustomAttribute<DisplayAttribute>();
                            propertValue = pv != null ? pv.Name : fieldInfo.Name;
                        }
                    }
                }
                else
                {
                    foreach (var fieldInfo in firstOrDefault.PropertyType.GetFields())
                    {
                        if (fieldInfo.Name == propertValue.ToString())
                        {
                            var pv = fieldInfo.GetCustomAttribute<DisplayAttribute>();
                            propertValue = pv != null ? pv.Name : fieldInfo.Name;
                        }
                    }
                }
            }
            else if (firstOrDefault.PropertyType == typeof(DateTime?) || firstOrDefault.PropertyType == typeof(DateTime))
            {
                if (!string.IsNullOrWhiteSpace(propertValue.ToString()))
                {
                    var d = DateTime.Parse(propertValue.ToString());
                    if (d.Year != 0001)
                    {
                        propertValue = d.ToShortShamsi(false);
                    }
                }
            }

            return string.IsNullOrWhiteSpace(propertValue.ToString()) ? "-" : propertValue;
        }

    }

    public class DefaultInfoForData
    {
        protected DefaultInfoForOptions Options;

        public DefaultInfoForData(DefaultInfoForOptions options)
        {
            Options = options;
        }

        public DefaultInfoForData AddFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, string propertyTitle = null,
            string customTemplate = null, string listPropertyName = null, string booleanOnTrueTemplate = null,
            string booleanOnFalseTemplate = null, int? colSize = null)
        {
            var forInfo = new DefaultInfoForItem()
            {
                Model = model,
                PropertyTitle = propertyTitle,
                CustomTemplate = customTemplate,
                ListPropertyName = listPropertyName,
                BooleanOnFalseTemplate = booleanOnFalseTemplate,
                BooleanOnTrueTemplate = booleanOnTrueTemplate,
                ColSize = colSize,
            };

            if (!string.IsNullOrWhiteSpace(booleanOnTrueTemplate) || 
                !string.IsNullOrWhiteSpace(booleanOnFalseTemplate))
            {
                forInfo.IsBoolean = true;
            }

            if (!string.IsNullOrWhiteSpace(listPropertyName))
            {
                forInfo.IsList = true;
            }

            var expBodyType = expression.Body.GetType();
            if (expBodyType.Name == $"{typeof(MethodCallExpression).Name}N")
            {
                forInfo.IsMultiProperty = true;
                forInfo.MultiPropertyName = new List<string>();

                try
                {
                    var arguments = ((MethodCallExpression)expression.Body).Arguments;

                    forInfo.MultiPropertyFormat = arguments[0].ToString();

                    foreach (var item in arguments.Skip(1).ToList())
                    {
                        forInfo.MultiPropertyName.Add(((MemberExpression)item).Member.Name);
                    }
                }
                catch (Exception e)
                {
                    e.LogErrorForElmah();
                }
            }
            else
            {
                forInfo.PropertyName = expression.GetPropertyName();
            }

            Options.DefaultInfoForItems.Add(forInfo);

            return this;
        }

        public DefaultInfoForData SetOptions(string title = null, DefaultInfoForColorType? color = null,
            DefaultInfoForColumeType? colume = null, bool? setAuthorize = null,
            HaveAccessToViewModel haveAccessTo = null)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Options.Title = title;
            }
            if (color != null)
            {
                Options.Color = color.Value;
            }
            if (colume != null)
            {
                Options.Colume = colume.Value;
            }
            if (setAuthorize != null)
            {
                Options.SetAuthorize = setAuthorize.Value;
            }
            if (haveAccessTo != null)
            {
                Options.HaveAccessTo = haveAccessTo;
            }
            return this;
        }

        /// <summary>
        /// ایجاد دکمه های جدید در فوتر
        /// </summary>
        /// <param name="haveAccessToContrller">نام کنترلر برای دسترسی</param>
        /// <param name="haveAccessToAction">نام اکشن برای دسترسی</param>
        /// <param name="text">عنوان دکمه</param>
        /// <param name="htmlAttributes">ویژگی های اچ تی ام ال</param>
        /// <returns></returns>
        public DefaultInfoForData SetFooterButton(string haveAccessToContrller, string haveAccessToAction, string text, Dictionary<string, string> htmlAttributes)
        {
            Options.EnablePanelFooter = true;
            Options.PanelFooterButtons.Add(new DefaultInfoForOptions.Button()
            {
                HtmlAttributes = htmlAttributes,
                Text = text,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToContrller = haveAccessToContrller,
            });
            return this;
        }

    }

    public class DefaultInfoForOptions
    {
        public DefaultInfoForOptions()
        {
            Colume = DefaultInfoForColumeType.Column3;
            Title = string.Empty;
            Color = DefaultInfoForColorType.Info;
            DefaultInfoForItems = new List<DefaultInfoForItem>();
            PanelFooterButtons = new List<Button>();
            SetAuthorize = true;
        }

        public HaveAccessToViewModel HaveAccessTo { get; set; }

        public bool SetAuthorize { get; set; }

        public string Title { get; set; }

        public DefaultInfoForColumeType Colume { get; internal set; }

        public DefaultInfoForColorType Color { get; set; }

        public List<DefaultInfoForItem> DefaultInfoForItems { get; set; }

        public bool EnablePanelFooter { get; set; }
        public List<Button> PanelFooterButtons { get; set; }

        public class Button
        {
            public string HaveAccessToContrller { get; set; }

            public string HaveAccessToAction { get; set; }

            public string Text { get; set; }

            public string IconClass { get; set; }

            public Dictionary<string, string> HtmlAttributes { get; set; }
        }

    }

    public class DefaultInfoForItem
    {
        /// <summary>
        /// مدل اطلاعات
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// نام پروپرتی مدل که قصد نمایش آن را داریم
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// عنوان پروپرتی برای نمایش در صفحه، به صورت پیش فرض از مدل استفاده می شود
        /// </summary>
        public string PropertyTitle { get; set; }

        /// <summary>
        /// نمایش سفارشی طلاعات
        /// </summary>
        public string CustomTemplate { get; set; }

        /// <summary>
        /// اطلاعات پروپرتی به صورت لیست
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// نام پروپرتی موجود در لیست جهت نمایش
        /// </summary>
        public string ListPropertyName { get; set; }

        /// <summary>
        /// پروپرتی بولی
        /// </summary>
        public bool IsBoolean { get; set; }

        /// <summary>
        /// نمایش سفارسی بولی در حالت درست
        /// </summary>
        public string BooleanOnTrueTemplate { get; set; }

        /// <summary>
        /// نمایش سفارشی بولی در حالت غلط
        /// </summary>
        public string BooleanOnFalseTemplate { get; set; }

        /// <summary>
        /// نمایش اطلاعات چند پروپرتی با هم
        /// </summary>
        public bool IsMultiProperty { get; set; }

        /// <summary>
        /// نام پروپرتی های چند تایی 
        /// </summary>
        public List<string> MultiPropertyName { get; set; }

        /// <summary>
        /// فرمت نمایش اطلاعات چند پروپرتی
        /// </summary>
        public string MultiPropertyFormat { get; set; }

        /// <summary>
        /// سایز صفحه بوت استرپ
        /// </summary>
        public int? ColSize { get; set; }

        /// <summary>
        /// سایز اسپم در جدول
        /// </summary>
        public int? Colspan { get; set; }
    }

    public enum DefaultInfoForColorType
    {
        [Display(Name = "default")]
        Default,
        [Display(Name = "success")]
        Success,
        [Display(Name = "warning")]
        Warning,
        [Display(Name = "info")]
        Info,
        [Display(Name = "danger")]
        Danger,
        [Display(Name = "primary")]
        Primary,
    }

    public enum DefaultInfoForColumeType
    {
        Column1 = 12,
        Column2 = 6,
        Column3 = 4,
        Column4 = 3,
        Column6 = 2,
    }
}