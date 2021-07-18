using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.Web.Framework.Helper.Enum;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class DefaultSearchFormForHelper
    {
        /// <summary>
        /// نمایش فرم جستجو
        /// </summary>
        /// <typeparam name="TModel">مدل اطلاعات</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="model">مدل صفحه</param>
        /// <param name="formPrefix">پیشوند نام تگ ها</param>
        /// <returns></returns>
        public static DefaultSearchFormForExtension DefaultSearchFormFor<TModel>
            (this HtmlHelper<TModel> htmlHelper, TModel model, string formPrefix = null)
        {
            return new DefaultSearchFormForExtension(htmlHelper: htmlHelper, model: model, formPrefix: formPrefix);
        }
    }

    public class DefaultSearchFormForExtension
    {
        private readonly HtmlHelper _htmlHelper;
        protected DefaultSearchFormForOptions Options;

        public DefaultSearchFormForExtension(HtmlHelper htmlHelper, object model, string formPrefix)
        {
            _htmlHelper = htmlHelper;
            var modelType = model.GetType();

            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();
            var actionName = _htmlHelper.ViewContext.RouteData.Values["action"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            Options = new DefaultSearchFormForOptions
            {
                Perfix = formPrefix,
                FormUrl = url.Action(actionName, controllerName, new {area = areaName}),
            };

            var dataPageTotal = modelType.GetProperty("PageTotal");
            if (dataPageTotal != null)
                Options.TotalNumber = (int)dataPageTotal.GetValue(model, null);

        }

        public DefaultSearchFormForExtension Items(Action<DefaultSearchFormForData> buildOptions)
        {
            buildOptions(new DefaultSearchFormForData(options: Options, htmlHelper: _htmlHelper));
            return this;
        }

        public MvcHtmlString Done()
        {
            var divWell = new TagBuilder("div") { InnerHtml = "" };
            divWell.AddCssClass("well well-shadow well-white");

            var divPageHeader = new TagBuilder("div") { InnerHtml = "" };
            divPageHeader.AddCssClass("page-header margin-bottom-20");

            var h4Title = new TagBuilder("h4") { InnerHtml = "" };
            h4Title.AddCssClass("margin-0 margin-bottom-10");
            h4Title.SetInnerText(Options.FormTitle);

            divPageHeader.InnerHtml = h4Title.ToString();

            divWell.InnerHtml += divPageHeader.ToString();

            var form = new TagBuilder("form");
            form.MergeAttribute("action", Options.FormUrl);
            form.MergeAttribute("method", "get");

            var list = new List<List<DefaultSearchFormForItem>>();
            var secondList = new List<DefaultSearchFormForItem>();
            var colume = 0;
            foreach (var item in Options.DefaultSearchFormForItems)
            {
                colume += item.ColumeSize;
                if (colume == 12)
                {
                    secondList.Add(item);
                    list.Add(secondList.Clone());
                    secondList.Clear();
                    colume = 0;
                }
                else if (colume < 12)
                {
                    secondList.Add(item);
                }
                else if (colume > 12)
                {
                    list.Add(secondList.Clone());
                    secondList.Clear();
                    secondList.Add(item);
                    colume = item.ColumeSize;
                }
            }

            if (secondList.Any())
            {
                list.Add(secondList);
            }

            var prefixName = string.Empty;
            var prefixId = string.Empty;
            if (!string.IsNullOrWhiteSpace(Options.Perfix))
            {
                prefixName = $"{Options.Perfix}.";
                prefixId = $"{Options.Perfix}_";
            }

            foreach (var itemRow in list)
            {
                var row = new TagBuilder("div") {InnerHtml = ""};
                row.AddCssClass("row margin-bottom-10");

                foreach (var item in itemRow)
                {
                    if (!string.IsNullOrWhiteSpace(item.ItemPerfix))
                    {
                        prefixName = $"{item.ItemPerfix}_";
                        prefixId = $"{item.ItemPerfix}.";
                    }

                    var divCol = new TagBuilder("div");
                    divCol.AddCssClass($"col-sm-{item.ColumeSize} margin-bottom-10");

                    var divFormGroup = new TagBuilder("form-group") {InnerHtml = ""};
                    divFormGroup.AddCssClass("form-group");

                    var label = new TagBuilder("label");
                    label.AddCssClass("control-label");
                    label.MergeAttribute("for", prefixId + item.PropertyName);
                    label.SetInnerText(item.PropertyText);

                    divFormGroup.InnerHtml += label.ToString();

                    if (item.DropDown != null)
                    {
                        var select = new TagBuilder("select"){InnerHtml = ""};
                        select.AddCssClass("form-control");
                        select.MergeAttribute("id", prefixId + item.PropertyName);
                        select.MergeAttribute("name", prefixName + item.PropertyName);

                        var optionChoose = new TagBuilder("option");
                        optionChoose.MergeAttribute("value", "");
                        optionChoose.SetInnerText("انتخاب کنید");

                        select.InnerHtml += optionChoose.ToString();

                        foreach (var itemOption in item.DropDown)
                        {
                            var option = new TagBuilder("option");
                            option.MergeAttribute("value", itemOption.Key);
                            option.SetInnerText(itemOption.Value);
                            if (item.PropertyValue == itemOption.Key)
                            {
                                option.MergeAttribute("selected", "selected");
                            }

                            select.InnerHtml += option.ToString();
                        }

                        divFormGroup.InnerHtml += select.ToString();
                    }
                    else if (!string.IsNullOrWhiteSpace(item.PropertyType) && item.PropertyType == "DateTime")
                    {
                        var tag = new TagBuilder("input");
                        tag.MergeAttribute("name", prefixName + item.PropertyName);
                        tag.MergeAttribute("id", prefixId + item.PropertyName);
                        tag.MergeAttribute("class", "form-control");
                        tag.MergeAttribute("data-mddatetimepicker", "true");
                        tag.MergeAttribute("data-trigger", "click");
                        tag.MergeAttribute("data-targetselector", "#" + prefixId + item.PropertyName);
                        tag.MergeAttribute("type", "text");

                        var datePickerValue = string.Empty;
                        if (!string.IsNullOrWhiteSpace(item.PropertyValue) && DateTime.Parse(item.PropertyValue).Year != 0001)
                        {
                            if (item.DatePickerType == DatePickerType.Miladi)
                            {
                                datePickerValue = DateTime.Parse(item.PropertyValue).ToString("MM/dd/yyyy");
                            }
                            else if (item.DatePickerType == DatePickerType.Shamsi)
                            {
                                datePickerValue = DateTime.Parse(item.PropertyValue).ToShortShamsi(false);
                            }
                            else
                            {
                                datePickerValue = DateTime.Parse(item.PropertyValue).ToShortShamsi(false);
                            }
                        }
                        tag.MergeAttribute("value", datePickerValue);

                        switch (item.DatePickerType)
                        {
                            case DatePickerType.Miladi:
                                tag.MergeAttribute("data-isgregorian", "true");
                                tag.MergeAttribute("data-mdformat", "MM/dd/yyyy");
                                break;
                            case DatePickerType.Shamsi:
                                tag.MergeAttribute("data-isgregorian", "false");
                                tag.MergeAttribute("data-mdformat", "yyyy/MM/dd");
                                break;
                            default:
                                tag.MergeAttribute("data-isgregorian", "false");
                                tag.MergeAttribute("data-mdformat", "yyyy/MM/dd");
                                break;
                        }

                        switch (item.DatePickerPlacement)
                        {
                            case DatePickerPlacement.Top:
                                tag.MergeAttribute("data-placement", "top");
                                break;
                            case DatePickerPlacement.Bottom:
                                tag.MergeAttribute("data-placement", "bottom");
                                break;
                            case DatePickerPlacement.Left:
                                tag.MergeAttribute("data-placement", "left");
                                break;
                            case DatePickerPlacement.Right:
                                tag.MergeAttribute("data-placement", "right");
                                break;
                            default:
                                tag.MergeAttribute("data-placement", "top");
                                break;
                        }

                        if (item.Validations.Any())
                            foreach (var itemValidation in item.Validations)
                                tag.MergeAttribute(itemValidation.Key, itemValidation.Value.ToString());

                        divFormGroup.InnerHtml += tag.ToString();
                    }
                    else if (!string.IsNullOrWhiteSpace(item.PropertyType) && item.PropertyType == "Boolean")
                    {
                        item.DropDown = new Dictionary<string, string>()
                        {
                            {true.ToString(), "فعال"},
                            {false.ToString(), "غیر فعال"},
                        };

                        var select = new TagBuilder("select") { InnerHtml = "" };
                        select.AddCssClass("form-control");
                        select.MergeAttribute("id", prefixId + item.PropertyName);
                        select.MergeAttribute("name", prefixName + item.PropertyName);

                        var optionChoose = new TagBuilder("option");
                        optionChoose.MergeAttribute("value", "");
                        optionChoose.SetInnerText("انتخاب کنید");

                        select.InnerHtml += optionChoose.ToString();

                        foreach (var itemOption in item.DropDown)
                        {
                            var option = new TagBuilder("option");
                            option.MergeAttribute("value", itemOption.Key);
                            option.SetInnerText(itemOption.Value);
                            if (item.PropertyValue == itemOption.Key)
                            {
                                option.MergeAttribute("selected", "selected");
                            }

                            select.InnerHtml += option.ToString();
                        }

                        divFormGroup.InnerHtml += select.ToString();
                    }
                    else
                    {
                        var input = new TagBuilder("input");
                        input.AddCssClass("form-control");
                        input.MergeAttribute("type", "text");
                        input.MergeAttribute("id", prefixId + item.PropertyName);
                        input.MergeAttribute("name", prefixName + item.PropertyName);
                        input.MergeAttribute("value", item.PropertyValue);
                        if (item.Validations.Any())
                            foreach (var itemValidation in item.Validations)
                                input.MergeAttribute(itemValidation.Key, itemValidation.Value.ToString());

                        divFormGroup.InnerHtml += input.ToString();
                    }

                    if (item.Validations.Any())
                    {
                        var spanValidation = new TagBuilder("span");
                        spanValidation.AddCssClass("field-validation-valid text-danger");
                        spanValidation.MergeAttribute("data-valmsg-for", prefixName + item.PropertyName);
                        spanValidation.MergeAttribute("data-valmsg-replace", "true");

                        divFormGroup.InnerHtml += spanValidation.ToString();
                    }

                    divCol.InnerHtml = divFormGroup.ToString();

                    row.InnerHtml += divCol.ToString();
                }

                form.InnerHtml += row.ToString();
            }

            var divFooterRow = new TagBuilder("div") { InnerHtml = "" };
            divFooterRow.AddCssClass("row");

            var divFooterRowCol6 = new TagBuilder("div") { InnerHtml = ""};
            divFooterRowCol6.AddCssClass("col-sm-6");

            var divFooterRowCol6Input = new TagBuilder("input");
            divFooterRowCol6Input.AddCssClass("btn btn-default");
            divFooterRowCol6Input.MergeAttribute("style", "margin-top:2px;");
            divFooterRowCol6Input.MergeAttribute("value", "جستجو کن!");
            divFooterRowCol6Input.MergeAttribute("type", "submit");

            divFooterRowCol6.InnerHtml += divFooterRowCol6Input.ToString();

            if (Options.ShowToralNumber)
            {
                var divFooterRowCol6Span = new TagBuilder("span");
                divFooterRowCol6Span.AddCssClass("btn btn-default disabled");
                divFooterRowCol6Span.MergeAttribute("style","margin:2px 10px 0 0;");
                divFooterRowCol6Span.SetInnerText("تعداد رکورد: " + Options.TotalNumber);

                divFooterRowCol6.InnerHtml += divFooterRowCol6Span.ToString();
            }

            divFooterRow.InnerHtml += divFooterRowCol6.ToString();

            form.InnerHtml += divFooterRow.ToString();

            divWell.InnerHtml += form.ToString();

            var html = divWell.ToString();

            return new MvcHtmlString(html);
        }

    }

    public class DefaultSearchFormForData
    {
        protected DefaultSearchFormForOptions Options;
        private readonly HtmlHelper _htmlHelper;

        public DefaultSearchFormForData(DefaultSearchFormForOptions options, HtmlHelper htmlHelper)
        {
            Options = options;
            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// ایجاد تگ تکس
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="columeSize">تعداد ستون های تگ اچ تی ام ال</param>
        /// <param name="prefix">پیشوند اسم و آیدی تگ ها</param>
        /// <returns></returns>
        public DefaultSearchFormForData AddFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize, string prefix = null)
        {
            var a = DefaultSearchFormForItem(model: model, expression: expression, columeSize: columeSize,
                prefix: prefix);

            Options.DefaultSearchFormForItems.Add(a);

            return this;
        }

        /// <summary>
        /// ایجاد تگ منوی کشویی 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="columeSize">تعداد ستون های تگ اچ تی ام ال</param>
        /// <param name="onTrue">عنوان منوی افتادنی در حالت درست</param>
        /// <param name="onFalse">عنوان منوی افتادنی در حالت غلط</param>
        /// <param name="prefix">پیشوند اسم و آیدی تگ ها</param>
        /// <returns></returns>
        public DefaultSearchFormForData AddBooleanDropDownFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize, string onTrue = "فعال", string onFalse = "غیر فعال", string prefix = null)
        {

            var a = DefaultSearchFormForItem(model: model, expression: expression, columeSize: columeSize,
                prefix: prefix);
            
            if (!string.IsNullOrWhiteSpace(onTrue) && !string.IsNullOrWhiteSpace(onFalse))
            {
                a.DropDown = new Dictionary<string, string>()
                {
                    {true.ToString(), onTrue},
                    {false.ToString(), onFalse},
                };
            }

            Options.DefaultSearchFormForItems.Add(a);

            return this;
        }

        /// <summary>
        /// ایجاد تگ منوی کشویی 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="columeSize">تعداد ستون های تگ اچ تی ام ال</param>
        /// <param name="dropDown">آیتم های تگ منوی کشویی</param>
        /// <param name="prefix">پیشوند اسم و آیدی تگ ها</param>
        /// <returns></returns>
        public DefaultSearchFormForData AddDropDownFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize, IDictionary<string, string> dropDown, string prefix = null)
        {
            var a = DefaultSearchFormForItem(model: model, expression: expression, columeSize: columeSize,
                prefix: prefix);

            a.DropDown = dropDown;

            Options.DefaultSearchFormForItems.Add(a);

            return this;
        }

        /// <summary>
        /// ایجاد تگ منوی کشویی 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="model"></param>
        /// <param name="expression"></param>
        /// <param name="columeSize">تعداد ستون های تگ اچ تی ام ال</param>
        /// <param name="datePickerPlacement"></param>
        /// <param name="prefix">پیشوند اسم و آیدی تگ ها</param>
        /// <param name="datePickerType"></param>
        /// <returns></returns>
        public DefaultSearchFormForData AddDatePickerFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize, DatePickerType datePickerType = DatePickerType.Shamsi, DatePickerPlacement datePickerPlacement = DatePickerPlacement.Top, string prefix = null)
        {
            var a = DefaultSearchFormForItem(model: model, expression: expression, columeSize: columeSize,
                prefix: prefix);

            a.DatePickerPlacement = datePickerPlacement;
            a.DatePickerType = datePickerType;

            Options.DefaultSearchFormForItems.Add(a);

            return this;
        }

        private DefaultSearchFormForItem DefaultSearchFormForItem<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression,
            int columeSize, string prefix)
        {
            var propertyName = expression.GetPropertyName();
            object propertyText = null;
            object propertyValue = null;
            IDictionary<string, object> validations = null;
            var propertyType = string.Empty;
            IDictionary<string, string> dropdown = null;

            var modelType = model.GetType();

            var firstOrDefault = modelType.GetProperties().FirstOrDefault(x => x.Name == expression.GetPropertyName());
            if (firstOrDefault != null)
            {
                var display = firstOrDefault.GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    propertyText = display.Name;

                propertyValue = firstOrDefault.GetValue(model, null);

                var fullname =
                    _htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                        partialFieldName: ExpressionHelper.GetExpressionText(expression));

                var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(model));
                validations = _htmlHelper.GetUnobtrusiveValidationAttributes(name: fullname, metadata: metadata);

                if (firstOrDefault.PropertyType == typeof(DateTime?) || firstOrDefault.PropertyType == typeof(DateTime))
                    propertyType = "DateTime";

                if (firstOrDefault.PropertyType == typeof(bool?) || firstOrDefault.PropertyType == typeof(bool))
                    propertyType = "Boolean";

                if (firstOrDefault.PropertyType.IsNullableEnum())
                {
                    propertyType = "Enum";
                    dropdown = new Dictionary<string, string>();

                    var answer = Nullable.GetUnderlyingType(firstOrDefault.PropertyType);
                    if (answer != null)
                    {
                        foreach (var fieldInfo in answer.GetFields())
                        {
                            if (fieldInfo.Name != "value__")
                            {
                                var value = fieldInfo.Name;
                                var attr = fieldInfo.GetCustomAttribute<DisplayAttribute>();
                                if (attr != null)
                                    value = attr.Name;

                                dropdown.Add(fieldInfo.Name, value);
                            }
                        }
                    }

                }
            }

            return new DefaultSearchFormForItem()
            {
                PropertyName = propertyName,
                PropertyText = propertyText?.ToString() ?? string.Empty,
                PropertyValue = propertyValue?.ToString() ?? string.Empty,
                ColumeSize = columeSize,
                ItemPerfix = prefix,
                Validations = validations ?? new Dictionary<string, object>(),
                PropertyType = propertyType,
                DropDown = dropdown,
            };
        }

        /// <summary>
        /// نمایش کادر تعداد رکوردها
        /// </summary>
        /// <param name="totalNumber">تعداد رکوردها</param>
        /// <param name="isEnabled">وضعیت نمایش</param>
        /// <returns></returns>
        public DefaultSearchFormForData SetTotalRecord(int totalNumber, bool isEnabled = true)
        {
            Options.TotalNumber = totalNumber;
            Options.ShowToralNumber = isEnabled;
            return this;
        }

        /// <summary>
        /// ذخیره مسیر ارسال درخواست و عنوان کادر جستجو
        /// </summary>
        /// <param name="formUrl">مسیر درخواست</param>
        /// <param name="formTitle">عنوان</param>
        /// <returns></returns>
        public DefaultSearchFormForData SetFormUrl(string formUrl, string formTitle = "جستجو")
        {
            Options.FormUrl = formUrl;
            Options.FormTitle = formTitle;
            return this;
        }

    }

    public class DefaultSearchFormForOptions
    {
        public DefaultSearchFormForOptions()
        {
            DefaultSearchFormForItems = new List<DefaultSearchFormForItem>();
            ShowToralNumber = true;
            FormTitle = "جستجو";
        }

        public List<DefaultSearchFormForItem> DefaultSearchFormForItems { get; set; }
        public string FormUrl { get; set; }
        public string FormTitle { get; set; }

        public bool ShowToralNumber { get; set; }
        public int TotalNumber { get; set; }

        public string Perfix { get; set; }
    }

    public class DefaultSearchFormForItem
    {
        public DefaultSearchFormForItem()
        {
            Validations = new Dictionary<string, object>();
        }

        public string PropertyName { get; set; }

        public string PropertyText { get; set; }

        public string PropertyValue { get; set; }

        public string PropertyType { get; set; }

        public int ColumeSize { get; set; }

        public string ItemPerfix { get; set; }

        public IDictionary<string, object> Validations { get; set; }

        public IDictionary<string, string> DropDown { get; set; }

        public DatePickerType? DatePickerType { get; set; }
        public DatePickerPlacement? DatePickerPlacement { get; set; }

    }

}