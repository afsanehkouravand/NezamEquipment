using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using NezamEquipment.Common.Extension;
using NezamEquipment.Web.Framework.Security;
using MvcPaging;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class DefaultGridForHelper
    {
        /// <summary>
        /// نمایش لیست اطلاعات در جدول
        /// </summary>
        /// <typeparam name="TModel">مدل اطلاعات</typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="title">عنوان جدول</param>
        /// <param name="color">رنگ جدول</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DefaultGridForExtension DefaultGridFor<TModel, TProperty>
            (this HtmlHelper<TModel> htmlHelper, TModel model, Expression<Func<TModel, TProperty>> expression, string title = "لیست اطلاعات", DefaultGridForColorType color = DefaultGridForColorType.Default)
        {
            var mType = model.GetType();

            IEnumerable<object> listOfData = null;
            HaveAccessToViewModel haveAccessTo = null;

            var dataListOfData = mType.GetProperty(expression.GetPropertyName());
            if (dataListOfData != null)
                listOfData = (IEnumerable<object>)dataListOfData.GetValue(model, null);

            var datahaveAccessTo = mType.GetProperty("HaveAccessTo");
            if (datahaveAccessTo != null)
                haveAccessTo = (HaveAccessToViewModel)datahaveAccessTo.GetValue(model, null);

            return new DefaultGridForExtension(htmlHelper: htmlHelper, model: model, listOfData: listOfData, haveAccessTo: haveAccessTo, title: title, color: color);
        }
    }

    public class DefaultGridForExtension
    {
        private readonly HtmlHelper _htmlHelper;
        protected DefaultGridForOptions Options;
        public object Model { get; set; }

        public DefaultGridForExtension(HtmlHelper htmlHelper, object model, IEnumerable<object> listOfData, HaveAccessToViewModel haveAccessTo, string title, DefaultGridForColorType color)
        {
            _htmlHelper = htmlHelper;
            Model = model;
            var modelType = model.GetType();

            Options = new DefaultGridForOptions
            {
                ListOfData = listOfData,
                HaveAccessTo = haveAccessTo,
                Title = title,
                Color = color
            };

            if (typeof(IPagingData).IsAssignableFrom(modelType))
            {
                var dataPageTotal = modelType.GetProperty("PageTotal");
                if (dataPageTotal != null)
                    Options.PageTotal = (int) dataPageTotal.GetValue(Model, null);

                var dataPageNumber = modelType.GetProperty("PageNumber");
                if (dataPageNumber != null)
                    Options.PageNumber = (int) dataPageNumber.GetValue(Model, null);

                var dataPageSize = modelType.GetProperty("PageSize");
                if (dataPageSize != null)
                    Options.PageSize = (int) dataPageSize.GetValue(Model, null);
            }
            else
            {
                Options.EnablePaging = false;
            }

            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            Options.DetailsUrl = url.Action("Details", controllerName, new {area = areaName});
            Options.EditUrl = url.Action("Edit", controllerName, new {area = areaName});
            Options.DeleteUrl = url.Action("Delete", controllerName, new {area = areaName});

        }

        public DefaultGridForExtension Items(Action<DefaultGridForData> buildOptions)
        {
            buildOptions(new DefaultGridForData(options: Options));
            return this;
        }

        public MvcHtmlString Done()
        {
            if (!Options.ListOfData.Any())
            {
                if (Options.ShowEmptyResultMessage)
                {
                    var divWell = new TagBuilder("div");
                    divWell.AddCssClass("well well-white well-shadow");

                    var divAlert = new TagBuilder("div");
                    divAlert.AddCssClass("alert alert-warning text-center margin-0");

                    var span = new TagBuilder("span");
                    span.SetInnerText(!string.IsNullOrWhiteSpace(Options.EmptyResultMessage) ? Options.EmptyResultMessage : "اطلاعاتی برای نمایش وجود ندارد.");

                    divAlert.InnerHtml = span.ToString();

                    divWell.InnerHtml = divAlert.ToString();

                    return new MvcHtmlString(divWell.ToString());
                }

                return new MvcHtmlString(string.Empty);
            }

            var divPanel = new TagBuilder("div") { InnerHtml = "" };
            divPanel.AddCssClass($"panel panel-{Options.Color.GetDisplayName()}");

            var rnd = new Random();
            var tableLoadingRandom = rnd.Next(1, 999999);

            var divPanelHeading = new TagBuilder("div"){InnerHtml = ""};
            divPanelHeading.AddCssClass("panel-heading");
            divPanelHeading.InnerHtml = Options.Title + "<i id=\"tableLoading"+ tableLoadingRandom + "\" class=\"fa fa-refresh fa-spin display-none\" style=\"margin-right:10px;\"></i>";

            var divTableResponsive = new TagBuilder("div"){ InnerHtml = ""};
            divTableResponsive.AddCssClass("table-responsive");
            divTableResponsive.MergeAttribute("style", "margin: 0 0 0 -1px;");

            var table = new TagBuilder("table");
            table.AddCssClass("table table-hover table-striped well well-white");

            #region Table Head

            var tableThead = new TagBuilder("thead");

            var tableTheadTr = new TagBuilder("tr") {InnerHtml = ""};

            if (Options.RowCounter)
            {
                var tableTheadTrThNmber = new TagBuilder("th");
                tableTheadTrThNmber.AddCssClass("text-center");
                tableTheadTrThNmber.SetInnerText("#");

                tableTheadTr.InnerHtml += tableTheadTrThNmber.ToString();
            }

            if (!string.IsNullOrWhiteSpace(Options.CheckBoxAttributeName))
            {
                var tableTheadTrThCheckBox = new TagBuilder("th");
                tableTheadTrThCheckBox.AddCssClass("text-center");

                var input = new TagBuilder("input");
                input.MergeAttribute("type", "checkbox");
                input.MergeAttribute("data-checkinputs", Options.CheckBoxAttributeName);

                tableTheadTrThCheckBox.InnerHtml = input.ToString();

                tableTheadTr.InnerHtml += tableTheadTrThCheckBox.ToString();
            }

            var tableTheadTrThButton = new TagBuilder("th");
            tableTheadTrThButton.AddCssClass("text-center");

            tableTheadTr.InnerHtml += tableTheadTrThButton.ToString();

            foreach (var item in Options.DefaultGridForItems)
            {
                var th = new TagBuilder("th");
                th.SetInnerText(item.PropertText);
                tableTheadTr.InnerHtml += th.ToString();
            }

            tableThead.InnerHtml = tableTheadTr.ToString();

            #endregion

            var tableTbody = new TagBuilder("tbody") {InnerHtml = ""};

            var n = (Options.PageNumber - 1) * Options.PageSize;

            foreach (var data in Options.ListOfData)
            {
                var dataType = data.GetType();

                long id = 0;
                var dataId = dataType.GetProperty("Id");
                if (dataId != null)
                    id = long.Parse(dataId.GetValue(data, null).ToString());

                var tr = new TagBuilder("tr"){ InnerHtml  = ""};
                tr.MergeAttribute("data-id",id.ToString());

                #region N Counter

                if (Options.RowCounter)
                {
                    var tdNumber = new TagBuilder("td");
                    tdNumber.AddCssClass("text-center");
                    tdNumber.SetInnerText((++n).ToString());

                    tr.InnerHtml += tdNumber.ToString();
                }

                #endregion

                #region CheckBox

                if (!string.IsNullOrWhiteSpace(Options.CheckBoxAttributeName))
                {
                    var tdCheckBox = new TagBuilder("td");
                    tdCheckBox.AddCssClass("text-center");

                    var input = new TagBuilder("input");
                    input.MergeAttribute("type", "checkbox");
                    input.MergeAttribute("name", Options.CheckBoxAttributeName);
                    input.MergeAttribute("data-highlight", "true");

                    if (!string.IsNullOrWhiteSpace(Options.CheckBoxAttributeClass))
                    {
                        input.MergeAttribute("class", Options.CheckBoxAttributeClass);
                    }

                    if (Options.CheckBoxAttributeValueFromModel == "Id")
                    {
                        input.MergeAttribute("value", id.ToString());
                    }
                    else
                    {
                        object value = null;
                        var propertyInfo = dataType.GetProperty(Options.CheckBoxAttributeValueFromModel);
                        if (propertyInfo != null)
                            value = propertyInfo.GetValue(data, null);

                        value = value ?? string.Empty;
                        input.MergeAttribute("value", value.ToString());
                    }

                    tdCheckBox.InnerHtml = input.ToString();

                    tr.InnerHtml += tdCheckBox.ToString();
                }

                #endregion

                #region Button

                var tdButton = new TagBuilder("td");
                tdButton.AddCssClass("text-center");
                tdButton.InnerHtml = "";

                var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

                #region Info Button

                var haveAccessToDetailsButton = true;
                if (Options.SetAuthorize)
                {
                    haveAccessToDetailsButton =
                    Options.HaveAccessTo.Check(
                        !string.IsNullOrWhiteSpace(Options.DetailsHaveAccessToController)
                            ? Options.DetailsHaveAccessToController
                            : controllerName, Options.DetailsHaveAccessToAction);
                }
                if (haveAccessToDetailsButton)
                {
                    if (!string.IsNullOrWhiteSpace(Options.DetailsUrl))
                    {
                        var a = new TagBuilder("a");
                        a.AddCssClass($"label label-{Options.DetailsColor}");
                        a.MergeAttribute("href", $"{Options.DetailsUrl}/{id}");
                        a.MergeAttribute("style", "margin:1px;");

                        var i = new TagBuilder("i");
                        i.AddCssClass("fa fa-info");

                        a.InnerHtml = i + " اطلاعات ";

                        tdButton.InnerHtml += a;
                    }
                }

                #endregion

                #region Edit Button

                var haveAccessToEditButton = true;
                if (Options.SetAuthorize)
                {
                    haveAccessToEditButton =
                        Options.HaveAccessTo.Check(
                            !string.IsNullOrWhiteSpace(Options.EditHaveAccessToController)
                                ? Options.EditHaveAccessToController
                                : controllerName, Options.EditHaveAccessToAction);
                }
                if (haveAccessToEditButton)
                {
                    if (!string.IsNullOrWhiteSpace(Options.EditUrl))
                    {
                        var a = new TagBuilder("a");
                        a.AddCssClass($"label label-{Options.EditColor}");
                        a.MergeAttribute("href", $"{Options.EditUrl}/{id}");
                        a.MergeAttribute("style", "margin:1px;");

                        var i = new TagBuilder("i");
                        i.AddCssClass("fa fa-edit");

                        a.InnerHtml = i + " ویرایش ";

                        tdButton.InnerHtml += a;
                    }
                }

                #endregion

                #region Delete Button

                var haveAccessToDeleteButton = true;
                if (Options.SetAuthorize)
                {
                    haveAccessToDeleteButton =
                        Options.HaveAccessTo.Check(
                            !string.IsNullOrWhiteSpace(Options.DeleteHaveAccessToController)
                                ? Options.DeleteHaveAccessToController
                                : controllerName, Options.DeleteHaveAccessToAction);
                }
                if (haveAccessToDeleteButton)
                {
                    if (!string.IsNullOrWhiteSpace(Options.DeleteUrl))
                    {
                        var a = new TagBuilder("a");
                        a.AddCssClass($"label label-{Options.DeleteColor} cursor-pointer");
                        a.MergeAttribute("style", "margin:1px;");
                        a.MergeAttribute("data-delete", "true");
                        a.MergeAttribute("data-id", id.ToString());
                        a.MergeAttribute("data-url", Options.DeleteUrl);
                        if (!string.IsNullOrWhiteSpace(Options.DeleteGoToPage))
                        {
                            a.MergeAttribute("data-gotopage", Options.DeleteGoToPage);
                        }

                        var i = new TagBuilder("i");
                        i.AddCssClass("fa fa-remove");

                        a.InnerHtml = i + " حذف ";

                        tdButton.InnerHtml += a;
                    }
                }

                #endregion

                #region Custom Button

                if (Options.TableButtons.Any())
                {
                    foreach (var item in Options.TableButtons)
                    {
                        var haveAccessTo = true;
                        if (Options.SetAuthorize)
                        {
                            haveAccessTo =
                                Options.HaveAccessTo.Check(item.HaveAccessToContrller, item.HaveAccessToAction);
                        }
                        if (haveAccessTo)
                        {
                            var a = new TagBuilder("a");
                            foreach (var attribute in item.HtmlAttributes)
                            {
                                var value = attribute.Value;
                                if (Regex.IsMatch(value, "\\[\\w+\\]"))
                                {
                                    var matchs = Regex.Match(value, "\\[\\w+\\]");
                                    foreach (var match in matchs.Groups)
                                    {
                                        object propertyValue = null;
                                        var name = match.ToString().Trim(Convert.ToChar("["), Convert.ToChar("]"));
                                        var propertyInfo = dataType.GetProperty(name);
                                        if (propertyInfo != null)
                                            propertyValue = propertyInfo.GetValue(data, null);

                                        if (propertyValue != null)
                                        {
                                            value = value.Replace(match.ToString(), propertyValue.ToString());
                                        }
                                    }
                                }

                                a.MergeAttribute(attribute.Key, value);
                            }

                            a.MergeAttribute("style", "margin:1px;");

                            if (!string.IsNullOrWhiteSpace(item.IconClass))
                            {
                                var i = new TagBuilder("i");
                                i.AddCssClass(item.IconClass);

                                a.InnerHtml = i + $" {item.Text} ";
                            }
                            else
                            {
                                a.SetInnerText(item.Text);
                            }

                            tdButton.InnerHtml += a;
                        }
                    }
                }

                #endregion

                tr.InnerHtml += tdButton.ToString();

                #endregion

                #region Table Colume

                var model = data.GetType().GetProperties();

                foreach (var item in Options.DefaultGridForItems)
                {
                    var firstOrDefault = model.FirstOrDefault(x => x.Name == item.PropertyName);
                    if (firstOrDefault != null)
                    {
                        var propertValue = firstOrDefault.GetValue(data, null);

                        if (!string.IsNullOrWhiteSpace(Options.TableTrColorfulPropertyBooleanName))
                        {
                            if (item.PropertyName == Options.TableTrColorfulPropertyBooleanName)
                            {
                                if (Options.TableTrColorfulColorRoles.Any(x => x.Key == propertValue.ToString()))
                                {
                                    var value = Options.TableTrColorfulColorRoles
                                        .FirstOrDefault(x => x.Key == propertValue.ToString())
                                        .Value;
                                    tr.MergeAttribute("class", value.GetDisplayName());
                                }
                            }
                        }

                        var td = new TagBuilder("td");

                        if (item.IsList == false || string.IsNullOrWhiteSpace(item.PropertyListName))
                        {
                            propertValue = propertValue?.ToString() ?? string.Empty;
                            propertValue = PropertValue(firstOrDefault, propertValue);

                            td.SetInnerText(propertValue.ToString());

                            if (item.IsBoolean)
                            {
                                var boolValue = bool.Parse(propertValue.ToString());
                                propertValue = boolValue ? item.BooleanTrueTemplate : item.BooleanFalseTemplate;
                                td.InnerHtml = propertValue.ToString();
                            }

                            if (item.IsCustomTemplate)
                            {
                                propertValue = string.Format(item.CustomTemplate, propertValue);
                                td.InnerHtml = propertValue.ToString();
                            }
                        }
                        else
                        {
                            var text = string.Empty;
                            var list = (IList) propertValue;
                            foreach (var l in list)
                            {
                                var value = l.GetType().GetProperty(item.PropertyListName).GetValue(l, null);
                                if (!string.IsNullOrWhiteSpace(item.CustomTemplate))
                                {
                                    text += string.Format(item.CustomTemplate, value);
                                }
                                else
                                {
                                    text += $" {value} ";
                                }
                            }
                            td.InnerHtml = text;
                        }


                        tr.InnerHtml += td.ToString();
                    }
                }

                #endregion

                tableTbody.InnerHtml += tr.ToString();
            }

            table.InnerHtml = tableThead + tableTbody.ToString();

            divTableResponsive.InnerHtml += table.ToString();

            divPanel.InnerHtml += divPanelHeading + divTableResponsive.ToString();

            #region Footer

            if (Options.FooterButtons.Any() || !string.IsNullOrWhiteSpace(Options.FooterDropDownName))
            {
                var divPanelFooter = new TagBuilder("div");
                divPanelFooter.AddCssClass("panel-footer");

                if (!string.IsNullOrWhiteSpace(Options.FooterDropDownName))
                {
                    var select = new TagBuilder("select") { InnerHtml = "" };
                    select.MergeAttribute("id", Options.FooterDropDownName);
                    select.MergeAttribute("name", Options.FooterDropDownName);
                    foreach (var item in Options.FooterDropDownHtmlAttributes)
                    {
                        select.MergeAttribute(item.Key, item.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(Options.FooterDropDownIsRequiredMessage))
                    {
                        select.MergeAttribute("data-val", "true");
                        select.MergeAttribute("data-val-required", Options.FooterDropDownIsRequiredMessage);
                    }

                    var optionChoose = new TagBuilder("option");
                    optionChoose.MergeAttribute("value", "");
                    optionChoose.SetInnerText("انتخاب کنید");

                    select.InnerHtml += optionChoose.ToString();

                    foreach (var itemOption in Options.FooterDropDownOptions)
                    {
                        var option = new TagBuilder("option");
                        option.MergeAttribute("value", itemOption.Key);
                        option.SetInnerText(itemOption.Value);
                        if (Options.FooterDropDownDefaultValue == itemOption.Key)
                        {
                            option.MergeAttribute("selected", "selected");
                        }

                        select.InnerHtml += option.ToString();
                    }

                    var spanValidation = new TagBuilder("span");
                    spanValidation.AddCssClass("field-validation-valid text-danger col-sm-12");
                    spanValidation.MergeAttribute("data-valmsg-for", Options.FooterDropDownName);
                    spanValidation.MergeAttribute("data-valmsg-replace", "true");

                    divPanelFooter.InnerHtml += spanValidation.ToString();

                    divPanelFooter.InnerHtml += select.ToString();

                }

                if (Options.FooterButtons.Any())
                {
                    foreach (var item in Options.FooterButtons)
                    {
                        var haveAccessTo = true;
                        if (Options.SetAuthorize)
                        {
                            haveAccessTo = Options.HaveAccessTo.Check(item.HaveAccessToContrller, item.HaveAccessToAction);
                        }
                        if (haveAccessTo)
                        {
                            var tag = new TagBuilder(item.TagType);
                            foreach (var attribute in item.HtmlAttributes)
                            {
                                tag.MergeAttribute(attribute.Key, attribute.Value);
                            }

                            tag.SetInnerText(item.Text);

                            divPanelFooter.InnerHtml += tag;
                        }
                    }
                }

                divPanel.InnerHtml += divPanelFooter.ToString();
            }

            #endregion

            var html = divPanel.ToString();

            #region form

            if (!string.IsNullOrWhiteSpace(Options.FormUrl))
            {
                var haveAccessTo = true;
                if (Options.SetAuthorize)
                {
                    haveAccessTo = Options.HaveAccessTo.Check(Options.FormHaveAccessToController,
                        Options.FormHaveAccessToAction);
                }
                if (haveAccessTo)
                {
                    var form = new TagBuilder("form") { InnerHtml = "" };
                    form.MergeAttribute("action", Options.FormUrl);
                    form.MergeAttribute("data-ajax", "true");
                    form.MergeAttribute("data-ajax-begin", "OnBegin");
                    form.MergeAttribute("data-ajax-complete", "OnComplete");
                    form.MergeAttribute("data-ajax-failure", "OnFailure");
                    form.MergeAttribute("data-ajax-success", "OnSuccess");
                    form.MergeAttribute("data-ajax-url", Options.FormUrl);
                    form.MergeAttribute("data-ajax-loading", "#tableLoading" + tableLoadingRandom);
                    form.MergeAttribute("data-ajax-method", "post");
                    form.MergeAttribute("method", "post");

                    form.InnerHtml += _htmlHelper.AntiForgeryToken();

                    if (Options.FormHiddenInput != null && Options.FormHiddenInput.Any())
                    {
                        foreach (var item in Options.FormHiddenInput)
                        {
                            var input = new TagBuilder("input");
                            input.MergeAttribute("type", "hidden");
                            input.MergeAttribute("name", item.Key);
                            input.MergeAttribute("value", item.Value);

                            form.InnerHtml += input.ToString();
                        }
                    }

                    var fieldset = new TagBuilder("fieldset");
                    fieldset.AddCssClass("fieldset");
                    fieldset.InnerHtml = html;

                    form.InnerHtml += fieldset.ToString();

                    html = form.ToString();
                }
            }


            #endregion

            #region paging

            if (Options.EnablePaging)
            {
                var paging = ListFooter(htmlHelper: _htmlHelper, pageTotal: Options.PageTotal,
                    pageSize: Options.PageSize, pageNumber: Options.PageNumber, enablePageSize: Options.EnablePageSize);
                return new MvcHtmlString(html + paging);
            }

            #endregion

            return new MvcHtmlString(html);
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
                            propertValue = fieldInfo.GetCustomAttribute<DisplayAttribute>().Name;
                        }
                    }
                }
                else
                {
                    foreach (var fieldInfo in firstOrDefault.PropertyType.GetFields())
                    {
                        if (fieldInfo.Name == propertValue.ToString())
                        {
                            propertValue = fieldInfo.GetCustomAttribute<DisplayAttribute>().Name;
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

        private static MvcHtmlString ListFooter(HtmlHelper htmlHelper, int pageTotal, int pageSize, int pageNumber, bool enablePageSize = true)
        {
            var divCol12 = new TagBuilder("div"){ InnerHtml = ""};
            divCol12.AddCssClass("row");

            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var controller = htmlHelper.ViewContext.RouteData.Values["controller"].ToString().ToLower();
            var action = htmlHelper.ViewContext.RouteData.Values["action"].ToString().ToLower();

            var requestQueryString = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            var getCollection = new FormCollection(requestQueryString);

            var areaName = htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();

            var queryStringPageNumber = new RouteValueDictionary() { { "area", areaName } };
            foreach (string item in getCollection)
            {
                queryStringPageNumber.Add(item.ToLower(), requestQueryString.Get(item.ToLower()));
            }

            if (queryStringPageNumber.All(x => x.Key != "pagesize"))
            {
                queryStringPageNumber.Add("pagesize", pageSize);
            }
            else
            {
                queryStringPageNumber["pageSize"] = pageSize;
            }

            if (queryStringPageNumber.Any(x => x.Key == "page"))
            {
                queryStringPageNumber.Remove("page");
            }

            var queryStringPaging = new RouteValueDictionary();
            foreach (string item in getCollection)
            {
                queryStringPaging.Add(item.ToLower(), requestQueryString.Get(item.ToLower()));
            }
            if (queryStringPaging.Any(x => x.Key == "page"))
            {
                queryStringPaging.Remove("page");
            }
            if (queryStringPaging.All(x => x.Key != "pagesize"))
            {
                queryStringPaging.Add("pagesize", pageSize);
            }

            var divCol8 = new TagBuilder("div");
            divCol8.AddCssClass("col-md-8 text-right-rtl");
            divCol8.InnerHtml = htmlHelper.Pager(pageSize: pageSize, currentPage: pageNumber, totalItemCount: pageTotal).Options(o => o
                .DisplayTemplate("Bootstrap3Pagination").RouteValues(queryStringPaging)).ToHtmlString();

            divCol12.InnerHtml += divCol8;

            if (enablePageSize)
            {
                var listForPageNumber = new Dictionary<int, string>()
                {
                    {10,"10" },
                    {20,"20" },
                    {50,"50" },
                    {1000,"1000" },
                };

                var divCol4 = new TagBuilder("div");
                divCol4.AddCssClass("col-md-4 hidden-sm hidden-xs text-left");

                var divBtnGroup = new TagBuilder("div");
                divBtnGroup.AddCssClass("btn-group");

                foreach (var item in listForPageNumber)
                {
                    queryStringPageNumber["pageSize"] = item.Key;

                    var aTag = new TagBuilder("a");
                    aTag.AddCssClass("btn btn-default btn-sm");
                    if (pageSize == item.Key)
                        aTag.AddCssClass("active");

                    aTag.MergeAttribute("href", url.Action(action, controller, queryStringPageNumber));

                    aTag.SetInnerText(item.Value);

                    divBtnGroup.InnerHtml += aTag.ToString();
                }

                divCol4.InnerHtml = divBtnGroup.ToString();
                divCol12.InnerHtml += divCol4;
            }

            return new MvcHtmlString(divCol12.ToString());
        }

    }

    public class DefaultGridForData
    {
        protected DefaultGridForOptions Options;

        public DefaultGridForData(DefaultGridForOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// ثبت ستون جدید
        /// </summary>
        /// <typeparam name="TModel">مدل جدول</typeparam>
        /// <typeparam name="TProperty">ویژگی مدل</typeparam>
        /// <param name="model">مدل</param>
        /// <param name="expression">ویژگی</param>
        /// <param name="customTemplate">سفارشی سازی نمایش اطلاعات</param>
        /// <returns></returns>
        public DefaultGridForData AddFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, string customTemplate = null)
        {
            var propertyName = expression.GetPropertyName();
            object propertText = null;

            var firstOrDefault = model.GetType().GetProperties().FirstOrDefault(x => x.Name == expression.GetPropertyName());
            if (firstOrDefault != null)
            {
                var display = firstOrDefault.GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    propertText = display.Name;
            }

            var a = new DefaultGridForItem()
            {
                PropertyName = propertyName,
                PropertText = propertText?.ToString() ?? string.Empty,
            };

            if (!string.IsNullOrWhiteSpace(customTemplate))
            {
                a.IsCustomTemplate = true;
                a.CustomTemplate = customTemplate;
            }

            Options.DefaultGridForItems.Add(a);

            return this;
        }

        /// <summary>
        /// ثبت ستون جدید به صورت لیست
        /// </summary>
        /// <typeparam name="TModel">مدل جدول</typeparam>
        /// <typeparam name="TProperty">ویژگی مدل</typeparam>
        /// <param name="model">مدل</param>
        /// <param name="expression">ویژگی</param>
        /// <param name="propertyListName">نام ویژگی مورد نظر در لیست برای نمایش اطلاعات  آن</param>
        /// <param name="customTemplate">سفارشی سازی نمایش اطلاعات</param>
        /// <returns></returns>
        public DefaultGridForData AddListFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression, string propertyListName, string customTemplate = null)
        {
            var propertyName = expression.GetPropertyName();
            object propertText = null;

            var firstOrDefault = model.GetType().GetProperties().FirstOrDefault(x => x.Name == expression.GetPropertyName());
            if (firstOrDefault != null)
            {
                var display = firstOrDefault.GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    propertText = display.Name;
            }

            var a = new DefaultGridForItem()
            {
                PropertyName = propertyName,
                PropertText = propertText?.ToString() ?? string.Empty,
                PropertyListName = propertyListName,
                CustomTemplate = customTemplate,
                IsList = true,
            };

            Options.DefaultGridForItems.Add(a);

            return this;
        }

        /// <summary>
        /// ثبت ستون جدید برای شرط
        /// </summary>
        /// <typeparam name="TModel">مدل جدول</typeparam>
        /// <typeparam name="TProperty">ویژگی مدل</typeparam>
        /// <param name="model">مدل</param>
        /// <param name="expression">ویژگی</param>
        /// <param name="onTrueTemplate">سفارشی سازی برای حالت درست</param>
        /// <param name="onFalseTemplate">سفارسی سازی اطلاعات برای حالت غلط</param>
        /// <returns></returns>
        public DefaultGridForData AddBooleanFor<TModel, TProperty>
            (TModel model, Expression<Func<TModel, TProperty>> expression,
            string onTrueTemplate = "فعال", string onFalseTemplate = "غیر فعال")
        {
            var propertyName = expression.GetPropertyName();
            object propertText = null;

            var firstOrDefault = model.GetType().GetProperties().FirstOrDefault(x => x.Name == expression.GetPropertyName());
            if (firstOrDefault != null)
            {
                var display = firstOrDefault.GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                    propertText = display.Name;
            }

            var a = new DefaultGridForItem()
            {
                PropertyName = propertyName,
                PropertText = propertText?.ToString() ?? string.Empty,
                IsBoolean = true,
                BooleanFalseTemplate = onFalseTemplate,
                BooleanTrueTemplate = onTrueTemplate,
            };

            Options.DefaultGridForItems.Add(a);

            return this;
        }

        /// <summary>
        /// ذخیره عنوان جدول
        /// </summary>
        /// <param name="tableColor"></param>
        /// <param name="showEmptyResultMessage"></param>
        /// <param name="emptyResultMessage"></param>
        /// <param name="tableTitle"></param>
        /// <param name="hideRowButton">مخفی کردن دکمه های اطلاعات و ویرایش  و حذف</param>
        /// <param name="enabledAuthorize">اجازه برای بررسی دسترسی ها در بخش مدیریت</param>
        /// <param name="enableRowCounter">نمایش/مخفی کردن ستون شماره گذاری ردیفها</param>
        /// <returns></returns>
        public DefaultGridForData SetOptions(string tableTitle = null, DefaultGridForColorType? tableColor = null,
            bool? showEmptyResultMessage = null, string emptyResultMessage = null, bool? hideRowButton = null,
            bool? enabledAuthorize = null, bool? enableRowCounter = null)
        {
            if (!string.IsNullOrWhiteSpace(tableTitle))
            {
                Options.Title = tableTitle;
            }
            if (tableColor != null)
            {
                Options.Color = tableColor.Value;
            }
            if (showEmptyResultMessage != null)
            {
                Options.ShowEmptyResultMessage = showEmptyResultMessage.Value;
            }
            if (!string.IsNullOrWhiteSpace(emptyResultMessage))
            {
                Options.EmptyResultMessage = emptyResultMessage;
            }
            if (hideRowButton != null)
            {
                Options.DetailsUrl = string.Empty;
                Options.DeleteUrl = string.Empty;
                Options.EditUrl = string.Empty;
            }
            if (enabledAuthorize != null)
            {
                Options.SetAuthorize = enabledAuthorize.Value;
            }
            if (enableRowCounter != null)
            {
                Options.RowCounter = enableRowCounter.Value;
            }

            return this;
        }

        /// <summary>
        /// اطلاعات مربوط به صفحه بندی جدول
        /// </summary>
        /// <param name="enablePageSize">نمایش/عدم نمایش تعداد ردیف های جدول</param>
        /// <param name="enablePaging">نمایش/عدم نمایش صفحه بندی جدول</param>
        /// <param name="pageTotal">کل رکوردها</param>
        /// <param name="pageSize">تعداد رکورد ها در صفحه</param>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <returns></returns>
        public DefaultGridForData SetPagingData(bool? enablePaging = null, bool? enablePageSize = null,
            int? pageTotal = null, int? pageSize = null, int? pageNumber = null)
        {
            if (pageTotal != null)
                Options.PageTotal = pageTotal.Value;
            
            if (pageNumber != null)
                Options.PageNumber = pageNumber.Value;
            
            if (pageSize != null)
                Options.PageSize = pageSize.Value;
            
            if (enablePaging != null)
                Options.EnablePaging = enablePaging.Value;
            
            if (enablePageSize != null)
                Options.EnablePageSize = enablePageSize.Value;

            return this;
        }

        /// <summary>
        /// اطلاعات دکمه اطلاعات
        /// </summary>
        /// <param name="url">مسیر</param>
        /// <param name="color">رنگ</param>
        /// <param name="title">عنوان</param>
        /// <param name="haveAccessToAction">نام اکشن برای درخواست</param>
        /// <param name="haveAccessToController">نام کنترلر برای درخواست - پیش فرض کنترلر جاری</param>
        /// <returns></returns>
        public DefaultGridForData SetDetailsButton(string url, string color = "info", string title = "اطلاعات",
            string haveAccessToAction = "Details", string haveAccessToController = null)
        {
            Options.DetailsUrl = url;
            Options.DetailsColor = color;
            Options.DetailsTitle = title;
            Options.DetailsHaveAccessToAction = haveAccessToAction;
            Options.DetailsHaveAccessToController = haveAccessToController;
            return this;
        }

        /// <summary>
        /// اطلاعات دکمه ویرایش
        /// </summary>
        /// <param name="url">مسیر</param>
        /// <param name="color">رنگ</param>
        /// <param name="title">عنوان</param>
        /// <param name="haveAccessToAction">نام اکشن برای درخواست</param>
        /// <param name="haveAccessToController">نام کنترلر برای درخواست - پیش فرض کنترلر جاری</param>
        /// <returns></returns>
        public DefaultGridForData SetEditButton(string url, string color = "primary", string title = "ویرایش",
            string haveAccessToAction = "Edit", string haveAccessToController = null)
        {
            Options.EditUrl = url;
            Options.EditColor = color;
            Options.EditTitle = title;
            Options.EditHaveAccessToAction = haveAccessToAction;
            Options.EditHaveAccessToController = haveAccessToController;
            return this;
        }

        /// <summary>
        /// اطلاعات دکمه حذف
        /// </summary>
        /// <param name="url">مسیر</param>
        /// <param name="color">رنگ</param>
        /// <param name="title">عنوان</param>
        /// <param name="haveAccessToAction">نام اکشن برای درخواست</param>
        /// <param name="haveAccessToController">نام کنترلر برای درخواست - پیش فرض کنترلر جاری</param>
        /// <param name="gotopage">رفتن به صفحه مورد نظر بعد از حذف</param>
        /// <returns></returns>
        public DefaultGridForData SetDeleteButton(string url, string color = "danger", string title = "حذف",
            string haveAccessToAction = "Delete", string haveAccessToController = null, string gotopage = null)
        {
            Options.DeleteUrl = url;
            Options.DeleteColor = color;
            Options.DeleteTitle = title;
            Options.DeleteGoToPage = gotopage;
            Options.DeleteHaveAccessToAction = haveAccessToAction;
            Options.DeleteHaveAccessToController = haveAccessToController;
            return this;
        }

        /// <summary>
        /// ایجاد دکمه های جدید در جدول
        /// </summary>
        /// <param name="haveAccessToContrller">نام کنترلر برای دسترسی</param>
        /// <param name="haveAccessToAction">نام اکشن برای دسترسی</param>
        /// <param name="text">عنوان دکمه</param>
        /// <param name="htmlAttributes">ویژگی های اچ تی ام ال</param>
        /// <param name="iconClass">کلاس آیکون</param>
        /// <returns></returns>
        public DefaultGridForData SetTableButton(string haveAccessToContrller, string haveAccessToAction, string text, Dictionary<string, string> htmlAttributes, string iconClass = null)
        {
            Options.TableButtons.Add(new DefaultGridForOptions.Button()
            {
                HtmlAttributes = htmlAttributes,
                IconClass = iconClass,
                Text = text,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToContrller = haveAccessToContrller,
            });
            return this;
        }

        /// <summary>
        /// ایجاد دکمه های جدید در انتهای جدول
        /// </summary>
        /// <param name="haveAccessToContrller">نام کنترلر برای دسترسی</param>
        /// <param name="haveAccessToAction">نام اکشن برای دسترسی</param>
        /// <param name="text">عنوان دکمه</param>
        /// <param name="tagType">نوع تک اچ تی ام ال</param>
        /// <param name="htmlAttributes">ویژگی های اچ تی ام ال</param>
        /// <returns></returns>
        public DefaultGridForData SetFooterButton(string haveAccessToContrller, string haveAccessToAction, string text, string tagType, Dictionary<string, string> htmlAttributes)
        {
            Options.FooterButtons.Add(new DefaultGridForOptions.Button()
            {
                HtmlAttributes = htmlAttributes,
                Text = text,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToContrller = haveAccessToContrller,
                TagType = tagType,
            });
            return this;
        }

        /// <summary>
        /// ساخت منوی کشویی در فوتر جدول
        /// </summary>
        /// <param name="name">عنوان و آیدی منوی کشویی</param>
        /// <param name="options">آیتم های منوی کشویی</param>
        /// <param name="htmlAttributes">اچ تی ام ال های منوی کشویی</param>
        /// <param name="defaultValue">مقدار پیش فرض</param>
        /// <param name="isRequiredMessage">پیام الزامی بودن منوی کشویی</param>
        /// <returns></returns>
        public DefaultGridForData SetFooterDropDown(string name, IDictionary<string, string> options, IDictionary<string, string> htmlAttributes = null, string defaultValue = null, string isRequiredMessage = null)
        {
            Options.FooterDropDownIsRequiredMessage = isRequiredMessage;
            Options.FooterDropDownName = name;
            Options.FooterDropDownDefaultValue = defaultValue;
            Options.FooterDropDownOptions = options;
            Options.FooterDropDownHtmlAttributes = htmlAttributes;
            return this;
        }

        /// <summary>
        /// نمایش ستون چک باکس در جدول
        /// </summary>
        /// <param name="attributename">نام چک باکس ها</param>
        /// <param name="attributeValueFromModel">مقدار چک باکس ها براسا مدل وارد شده</param>
        /// <param name="attributeClass">ویژگی شی کلاس در اچ تی ام ال</param>
        /// <returns></returns>
        public DefaultGridForData SetCheckBoxColume(string attributename, string attributeValueFromModel = "Id", string attributeClass = null)
        {
            Options.CheckBoxAttributeClass = attributeClass;
            Options.CheckBoxAttributeName = attributename;
            Options.CheckBoxAttributeValueFromModel = attributeValueFromModel;
            return this;
        }

        /// <summary>
        /// محاصره جدول در تک فرم
        /// </summary>
        /// <param name="haveAccessToContrller">نام کنترلر ارسالی اطلاعات</param>
        /// <param name="haveAccessToAction">نام اکشن ارسالی اطلاعات</param>
        /// <param name="url">مسیر ارسال اطلاعات</param>
        /// <param name="hiddenInput">تک مخفی برای ارسال اطلاعات اضافه</param>
        /// <returns></returns>
        public DefaultGridForData SetForm(string haveAccessToContrller, string haveAccessToAction, string url, 
            Dictionary<string, string> hiddenInput = null)
        {
            Options.FormHaveAccessToAction = haveAccessToAction;
            Options.FormHaveAccessToController = haveAccessToContrller;
            Options.FormUrl = url;
            Options.FormHiddenInput = hiddenInput;
            return this;
        }

        /// <summary>
        /// رنگی کردن ردیف های جدول
        /// </summary>
        /// <param name="propertyBooleanName">نام پروپرتی مدل</param>
        /// <param name="colorRoles">قوانین رنگ گذاری ستون ها، کلید دیکشنری برابر مقدار پروپرتی و مقدار دیکشنری برابر رنگ ستون </param>
        /// <returns></returns>
        public DefaultGridForData SetTrColorfulRoles(string propertyBooleanName, IDictionary<string, DefaultGridForColorType> colorRoles)
        {
            Options.TableTrColorfulColorRoles = colorRoles;
            Options.TableTrColorfulPropertyBooleanName = propertyBooleanName;
            return this;
        }

    }

    public class DefaultGridForOptions
    {
        public DefaultGridForOptions()
        {
            RowCounter = true;
            DefaultGridForItems = new List<DefaultGridForItem>();
            ListOfData = new List<object>();
            DetailsColor = "info";
            EditColor = "primary";
            DeleteColor = "danger";
            TableButtons = new List<Button>();
            FooterButtons = new List<Button>();
            DetailsHaveAccessToAction = "Details";
            EditHaveAccessToAction = "Edit";
            DeleteHaveAccessToAction = "Delete";
            ShowEmptyResultMessage = true;
            SetAuthorize = true;
            EnablePaging = true;
            EnablePageSize = true;
        }

        public IDictionary<string, DefaultGridForColorType> TableTrColorfulColorRoles { get; set; }
        public string TableTrColorfulPropertyBooleanName { get; set; }

        public bool SetAuthorize { get; set; }

        public bool ShowEmptyResultMessage { get; set; }
        public string EmptyResultMessage { get; set; }

        public string FormUrl { get; set; }
        public string FormHaveAccessToController { get; set; }
        public string FormHaveAccessToAction { get; set; }
        public Dictionary<string, string> FormHiddenInput { get; set; }

        public string CheckBoxAttributeClass { get; set; }
        public string CheckBoxAttributeName { get; set; }
        public string CheckBoxAttributeValueFromModel { get; set; }

        public bool RowCounter { get; set; }

        public string Title { get; set; }

        public int Colume { get; internal set; }

        public DefaultGridForColorType Color { get; set; }

        public List<DefaultGridForItem> DefaultGridForItems { get; set; }

        public IEnumerable<object> ListOfData { get; set; }

        public bool EnablePaging { get; set; }
        public int PageTotal { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool EnablePageSize { get; set; }

        public string DetailsUrl { get; set; }
        public string DetailsColor { get; set; }
        public string DetailsTitle { get; set; }
        public string DetailsHaveAccessToController { get; set; }
        public string DetailsHaveAccessToAction { get; set; }

        public string EditUrl { get; set; }
        public string EditColor { get; set; }
        public string EditTitle { get; set; }
        public string EditHaveAccessToController { get; set; }
        public string EditHaveAccessToAction { get; set; }

        public string DeleteUrl { get; set; }
        public string DeleteColor { get; set; }
        public string DeleteTitle { get; set; }
        public string DeleteGoToPage { get; set; }
        public string DeleteHaveAccessToController { get; set; }
        public string DeleteHaveAccessToAction { get; set; }

        public HaveAccessToViewModel HaveAccessTo { get; set; }

        public List<Button> TableButtons { get; set; }

        public List<Button> FooterButtons { get; set; }

        public string FooterDropDownName { get; set; }
        public IDictionary<string, string> FooterDropDownHtmlAttributes { get; set; }
        public IDictionary<string, string> FooterDropDownOptions { get; set; }
        public string FooterDropDownDefaultValue { get; set; }
        public string FooterDropDownIsRequiredMessage { get; set; }
        
        public class Button
        {
            public string HaveAccessToContrller { get; set; }

            public string HaveAccessToAction { get; set; }

            public string Text { get; set; }

            public string IconClass { get; set; }

            public Dictionary<string, string> HtmlAttributes { get; set; }

            public string TagType { get; set; }
        }
    }

    public class DefaultGridForItem
    {
        public string PropertyName { get; set; }

        public string PropertText { get; set; }

        public string PropertyListName { get; set; }

        public bool IsList { get; set; }

        public bool IsBoolean { get; set; }

        public string BooleanTrueTemplate { get; set; }

        public string BooleanFalseTemplate { get; set; }

        public bool IsCustomTemplate { get; set; }

        public string CustomTemplate { get; set; }
    }

    public enum DefaultGridForColorType
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

    public interface IPagingData
    {
        /// <summary>
        /// تعداد کل ردیف ها
        /// </summary>
        int PageTotal { get; set; }
        /// <summary>
        /// تعداد ردیف های نمایش داده شده در جدول
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// شماره صفحه
        /// </summary>
        int PageNumber { get; set; }
    }

}