using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MyCommon.Helpers.Enum;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Helper
{
    public static class DefaultFormForHelper
    {
        public static DefaultFormForExtension DefaultFormFor<TModel>
            (this HtmlHelper<TModel> htmlHelper, string url, string title = null, object haveAccessTo = null,
            string haveAccessToControler = null, string haveAccessToAction = null, string perfix = null,
            string onBegin = "OnBegin", string onComplete = "OnComplete", string onFailure = "OnFailure",
            string onSuccess = "OnSuccess", string loading = "loading", string goToPage = null, bool? isShowingWell = null)
        {
            return new DefaultFormForExtension(htmlHelper: htmlHelper, url: url, haveAccessTo: haveAccessTo,
                title: title, haveAccessToControler: haveAccessToControler, haveAccessToAction: haveAccessToAction,
                perfix: perfix, onBegin: onBegin, onComplete: onComplete, onFailure: onFailure,
                onSuccess: onSuccess, loading: loading, goToPage: goToPage, isShowingWell: isShowingWell);
        }
    }

    public class DefaultFormForExtension
    {
        private readonly HtmlHelper _htmlHelper;
        protected DefaultFormForOptions Options;

        public DefaultFormForExtension(HtmlHelper htmlHelper, string url, string title = null, object haveAccessTo = null,
            string haveAccessToControler = null, string haveAccessToAction = null, string perfix = null,
            string onBegin = "OnBegin", string onComplete = "OnComplete", string onFailure = "OnFailure",
            string onSuccess = "OnSuccess", string loading = "loading", string goToPage = null, bool? isShowingWell = null)
        {
            _htmlHelper = htmlHelper;

            Options = new DefaultFormForOptions
            {
                Perfix = perfix,
                Url = url,
                HaveAccessTo = haveAccessTo,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToController = haveAccessToControler,
                OnComplete = onComplete,
                OnFailure = onFailure,
                OnBegin = onBegin,
                OnSuccess = onSuccess,
                Loading = loading,
                Title = title,
                GoToPage = goToPage,
                IsShowingWell = isShowingWell,
            };

        }

        public DefaultFormForExtension Items(Action<DefaultFormForData> buildOptions)
        {
            buildOptions(new DefaultFormForData(options: Options, htmlHelper: _htmlHelper));
            return this;
        }

        public MvcHtmlString Done()
        {
            var haveAccessTo = true;
            if (Options.HaveAccessTo != null)
            {
                haveAccessTo =
                    Options.HaveAccessTo.Check("Admin",
                    Options.HaveAccessToController, Options.HaveAccessToAction);
            }

            if (!haveAccessTo)
            {
                var div = new TagBuilder("div");

                if (Options.IsShowingWell != null && Options.IsShowingWell == true)
                    div.AddCssClass("well well-white well-shadow");

                var divAlert = new TagBuilder("div");
                divAlert.AddCssClass("alert alert-warning text-center margin-0");

                var span = new TagBuilder("span");
                span.SetInnerText("شما اجازه لازم را ندارید.");

                divAlert.InnerHtml = span.ToString();

                div.InnerHtml = divAlert.ToString();

                return new MvcHtmlString(div.ToString());
            }

            var divWell = new TagBuilder("div") { InnerHtml = "" };
            divWell.AddCssClass("well well-shadow well-white");

            if (!string.IsNullOrWhiteSpace(Options.Title))
            {
                var divPageHeader = new TagBuilder("div") { InnerHtml = "" };
                divPageHeader.AddCssClass("page-header margin-0 margin-top-10 margin-bottom-20");

                var h4Title = new TagBuilder("h4") { InnerHtml = "" };
                h4Title.AddCssClass("margin-0 margin-bottom-10");
                h4Title.SetInnerText(Options.Title);

                divPageHeader.InnerHtml += h4Title.ToString();

                divWell.InnerHtml += divPageHeader.ToString();
            }

            var form = new TagBuilder("form") { InnerHtml = "" };
            form.MergeAttribute("action", Options.Url);
            form.MergeAttribute("method", "post");
            form.MergeAttribute("data-ajax", "true");
            form.MergeAttribute("data-ajax-method", "post");
            form.MergeAttribute("data-ajax-loading", $"#{Options.Loading}");
            form.MergeAttribute("data-ajax-begin", Options.OnBegin);
            form.MergeAttribute("data-ajax-complete", Options.OnComplete);
            form.MergeAttribute("data-ajax-failure", Options.OnFailure);
            form.MergeAttribute("data-ajax-success", Options.OnSuccess);
            form.MergeAttribute("data-ajax-url", Options.Url);
            if (!string.IsNullOrWhiteSpace(Options.GoToPage))
            {
                form.MergeAttribute("data-gotopage", Options.GoToPage);
                form.MergeAttribute("onsubmit", "formonsubmit(this)");

            }

            form.InnerHtml += _htmlHelper.AntiForgeryToken();
            form.InnerHtml += _htmlHelper.ValidationSummary(true, "", new { @class = "text-danger" });

            var fieldset = new TagBuilder("fieldset");
            fieldset.AddCssClass("form-horizontal fieldset");

            foreach (var item in Options.HiddenProperties)
            {
                var key = !string.IsNullOrWhiteSpace(Options.Perfix) ? $"{Options.Perfix}.{item.Key}" : item.Key;
                fieldset.InnerHtml += _htmlHelper.Hidden(key, item.Value);
            }

            var list = new List<List<DefaultFormForItem>>();
            var secondList = new List<DefaultFormForItem>();
            var colume = 0;
            foreach (var item in Options.DefaultFormForItems)
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

            foreach (var itemRow in list)
            {
                var row = new TagBuilder("div") { InnerHtml = "" };
                row.AddCssClass("row margin-bottom-10");

                foreach (var item in itemRow)
                {
                    row.InnerHtml += item.Html;
                }

                fieldset.InnerHtml += row.ToString();
            }

            var divFooterRow = new TagBuilder("div") { InnerHtml = "" };
            divFooterRow.AddCssClass("row margin-top-20");

            var divFooterRowCol6 = new TagBuilder("div") { InnerHtml = "" };
            divFooterRowCol6.AddCssClass("col-sm-12");

            var divFooterRowCol6Input = new TagBuilder("input");
            divFooterRowCol6Input.AddCssClass("btn btn-success");
            divFooterRowCol6Input.MergeAttribute("style", "margin-top:2px;");
            divFooterRowCol6Input.MergeAttribute("value", "ارسال");
            divFooterRowCol6Input.MergeAttribute("type", "submit");

            divFooterRowCol6.InnerHtml += divFooterRowCol6Input.ToString();

            divFooterRowCol6.InnerHtml += _htmlHelper.ButtonLoading(Options.Loading);

            divFooterRow.InnerHtml += divFooterRowCol6.ToString();

            fieldset.InnerHtml += divFooterRow.ToString();

            form.InnerHtml += fieldset.ToString();

            divWell.InnerHtml += form.ToString();

            var html = divWell.ToString();

            return new MvcHtmlString(html);
        }
    }

    public class DefaultFormForData
    {
        protected DefaultFormForOptions Options;
        private readonly HtmlHelper _htmlHelper;

        public DefaultFormForData(DefaultFormForOptions options, HtmlHelper htmlHelper)
        {
            Options = options;
            _htmlHelper = htmlHelper;
        }

        public void HiddenProperty(string key, string value)
        {
            Options.HiddenProperties.Add(key, value);
        }

        public void HiddenProperties(IDictionary<string, string> hiddenItems)
        {
            if (hiddenItems == null)
                return;

            foreach (var item in hiddenItems)
            {
                Options.HiddenProperties.Add(item.Key, item.Value);
            }
        }

        public void GoToPage(string url)
        {
            Options.GoToPage = url;
        }

        public void HaveAccessTo(object haveAccessTo, string controller, string action)
        {
            Options.HaveAccessTo = haveAccessTo;
            Options.HaveAccessToController = controller;
            Options.HaveAccessToAction = action;
        }

        public void AddLine(string style = "margin:10px 0 5px", string @class = null)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("col-sm-12");

            var hr = new TagBuilder("hr");
            hr.MergeAttribute("style", style);
            hr.AddCssClass(@class);

            div.InnerHtml += hr.ToString();

            Options.DefaultFormForItems.Add(new DefaultFormForItem()
            {
                Html = div.ToString(),
                ColumeSize = 12,
            });
        }

        public void AddTitle(string title, string @class = "margin-0")
        {
            var div = new TagBuilder("div");
            div.AddCssClass("col-sm-12");

            var legend = new TagBuilder("legend");
            legend.AddCssClass(@class);
            legend.SetInnerText(title);

            div.InnerHtml += legend.ToString();

            Options.DefaultFormForItems.Add(new DefaultFormForItem()
            {
                Html = div.ToString(),
                ColumeSize = 12,
            });
        }

        public void AddTextBoxFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize)
        {
            var modelType = model.GetType();
            var expressionName = expression.GetPropertyName();

            var propertyInfo = modelType.GetProperty(expressionName);
            if (propertyInfo == null)
                return;

            var expressionNamePerfix = !string.IsNullOrWhiteSpace(Options.Perfix)
                ? $"{Options.Perfix}.{expressionName}"
                : expressionName;

            var value = propertyInfo.GetValue(model, null);

            if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?) ||
                propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
            {
                if (value != null)
                {
                    var valueString = value.ToString();
                    if (valueString == "0")
                    {
                        value = string.Empty;
                    }
                }
            }

            var html = _htmlHelper.TextBox(expressionNamePerfix, value, new { @class = "form-control" });

            AddHtml(modelType, expressionName, columeSize, html.ToString(), propertyInfo: propertyInfo);
        }

        public void AddTextAreaFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize)
        {
            var modelType = model.GetType();
            var expressionName = expression.GetPropertyName();

            var propertyInfo = modelType.GetProperty(expressionName);
            if (propertyInfo == null)
                return;

            var expressionNamePerfix = !string.IsNullOrWhiteSpace(Options.Perfix)
                ? $"{Options.Perfix}.{expressionName}"
                : expressionName;

            var value = propertyInfo.GetValue(model, null);
            var html = _htmlHelper.TextArea(expressionNamePerfix, value?.ToString() ?? string.Empty, new { @class = "form-control" });

            AddHtml(modelType, expressionName, columeSize, html.ToString(), propertyInfo: propertyInfo);
        }

        public void AddEnumDropDownFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize)
        {
            var modelType = model.GetType();
            var expressionName = expression.GetPropertyName();

            var propertyInfo = modelType.GetProperty(expressionName);
            if (propertyInfo == null)
                return;

            var expressionNamePerfix = !string.IsNullOrWhiteSpace(Options.Perfix)
                ? $"{Options.Perfix}.{expressionName}"
                : expressionName;

            var mydic = propertyInfo.AnonymousEnumToDictionary();

            var html = _htmlHelper.DropDownList(expressionNamePerfix, GetDropDownExtension.ByDictionary(mydic),
                new { @class = "form-control" });

            AddHtml(modelType, expressionName, columeSize, html.ToString(), propertyInfo: propertyInfo);
        }

        public void AddDropDownFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize,
            IDictionary<string, string> list)
        {
            var modelType = model.GetType();
            var expressionName = expression.GetPropertyName();

            var expressionNamePerfix = !string.IsNullOrWhiteSpace(Options.Perfix)
                ? $"{Options.Perfix}.{expressionName}"
                : expressionName;

            var html = _htmlHelper.DropDownList(expressionNamePerfix, GetDropDownExtension.ByDictionary(list),
                new { @class = "form-control" });

            AddHtml(modelType, expressionName, columeSize, html.ToString());
        }

        public void AddBooleanFor<TModel, TProperty>
        (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize,
            string onTrue = "فعال", string onFalse = "غیر فعال")
        {
            var modelType = model.GetType();
            var expressionName = expression.GetPropertyName();

            var expressionNamePerfix = !string.IsNullOrWhiteSpace(Options.Perfix)
                ? $"{Options.Perfix}.{expressionName}"
                : expressionName;

            var mydic = new Dictionary<bool, string>()
            {
                {true, onTrue},
                {false, onFalse},
            };

            var html = _htmlHelper.DropDownList(expressionNamePerfix, GetDropDownExtension.ByDictionary(mydic),
                new { @class = "form-control" });

            AddHtml(modelType, expressionName, columeSize, html.ToString());
        }

        public void AddDatePickerFor<TModel, TProperty>
        (TModel model, Expression<Func<TModel, TProperty>> expression, int columeSize,
            DatePickerType datePickerType = DatePickerType.Shamsi,
            DatePickerPlacement datePickerPlacement = DatePickerPlacement.Top)
        {
            var modelType = model.GetType();
            var expressionName = expression.GetPropertyName();

            var propertyInfo = modelType.GetProperty(expressionName);
            if (propertyInfo == null)
                return;

            var value = propertyInfo.GetValue(model, null);
            var itemPropertyValue = value?.ToString() ?? string.Empty;

            var prefixName = string.Empty;
            var prefixId = string.Empty;
            if (!string.IsNullOrWhiteSpace(Options.Perfix))
            {
                prefixName = $"{Options.Perfix}.";
                prefixId = $"{Options.Perfix}_";
            }

            var tag = new TagBuilder("input");
            tag.MergeAttribute("name", prefixName + expressionName);
            tag.MergeAttribute("id", prefixId + expressionName);
            tag.MergeAttribute("class", "form-control");
            tag.MergeAttribute("data-mddatetimepicker", "true");
            tag.MergeAttribute("data-trigger", "click");
            tag.MergeAttribute("data-targetselector", "#" + prefixId + expressionName);
            tag.MergeAttribute("type", "text");

            var datePickerValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(itemPropertyValue) && DateTime.Parse(itemPropertyValue).Year != 0001)
            {
                if (datePickerType == DatePickerType.Miladi)
                {
                    datePickerValue = DateTime.Parse(itemPropertyValue).ToString("MM/dd/yyyy");
                }
                else if (datePickerType == DatePickerType.Shamsi)
                {
                    datePickerValue = DateTime.Parse(itemPropertyValue).ToShortShamsi();
                }
                else
                {
                    datePickerValue = DateTime.Parse(itemPropertyValue).ToShortShamsi();
                }
            }
            tag.MergeAttribute("value", datePickerValue);

            switch (datePickerType)
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

            switch (datePickerPlacement)
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


            var fullname =
                _htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                    partialFieldName: ExpressionHelper.GetExpressionText(expression));

            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(model));
            var validations = _htmlHelper.GetUnobtrusiveValidationAttributes(name: fullname, metadata: metadata);

            if (validations.Any())
                foreach (var itemValidation in validations)
                    tag.MergeAttribute(itemValidation.Key, itemValidation.Value.ToString());

            AddHtml(modelType, expressionName, columeSize, tag.ToString());
        }


        private void AddHtml(Type modelType, string expressionName, int columeSize, string propertyHtml,
            PropertyInfo propertyInfo = null)
        {
            if (propertyInfo == null)
            {
                propertyInfo = modelType.GetProperty(expressionName);
                if (propertyInfo == null)
                    return;
            }

            var divTag = new TagBuilder("div") { InnerHtml = "" };
            divTag.AddCssClass($"col-sm-{columeSize}");


            var label = expressionName;
            var displayName = propertyInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayName != null)
                label = displayName.Name;

            divTag.InnerHtml += _htmlHelper.Label(label, new { @class = "control-label" });

            var expressionNamePerfix = !string.IsNullOrWhiteSpace(Options.Perfix)
                ? $"{Options.Perfix}.{expressionName}"
                : expressionName;

            divTag.InnerHtml += propertyHtml;

            divTag.InnerHtml += _htmlHelper.ValidationMessage(expressionNamePerfix, "", new { @class = "text-danger" });

            Options.DefaultFormForItems.Add(new DefaultFormForItem()
            {
                Html = divTag.ToString(),
                ColumeSize = columeSize,
            });
        }

    }

    public class DefaultFormForOptions
    {
        public DefaultFormForOptions()
        {
            HiddenProperties = new Dictionary<string, string>();
            DefaultFormForItems = new List<DefaultFormForItem>();
        }

        public string OnSuccess { get; set; }
        public string OnFailure { get; set; }
        public string OnComplete { get; set; }
        public string OnBegin { get; set; }
        public string Loading { get; set; }

        public string Url { get; set; }
        public string Title { get; set; }

        public string Perfix { get; set; }

        public string HaveAccessToController { get; set; }
        public string HaveAccessToAction { get; set; }
        public object HaveAccessTo { get; set; }

        public IDictionary<string, string> HiddenProperties { get; set; }

        public IList<DefaultFormForItem> DefaultFormForItems { get; set; }

        public string GoToPage { get; set; }

        public bool? IsShowingWell { get; set; }
    }

    public class DefaultFormForItem
    {
        public int ColumeSize { get; set; }

        public string Html { get; set; }
    }

}