using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPaging;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Grid
{
    public class GridExtension
    {
        private static HtmlHelper _htmlHelper;
        private static GridOption _option;

        public GridExtension(HtmlHelper htmlHelper, object model, IEnumerable<object> listOfData, 
            string title, EGridColorType color)
        {
            _htmlHelper = htmlHelper;
            var modelType = model.GetType();

            _option = new GridOption
            {
                ListOfData = listOfData,
                TableTitle = title,
                TableColor = color,
                RequestContext = _htmlHelper.ViewContext.RequestContext,
            };

            if (typeof(IPagingData).IsAssignableFrom(modelType))
            {
                var dataPageTotal = modelType.GetProperty("PageTotal");
                if (dataPageTotal != null)
                    _option.PageTotal = (int) dataPageTotal.GetValue(model, null);

                var dataPageNumber = modelType.GetProperty("PageNumber");
                if (dataPageNumber != null)
                    _option.PageNumber = (int) dataPageNumber.GetValue(model, null);

                var dataPageSize = modelType.GetProperty("PageSize");
                if (dataPageSize != null)
                    _option.PageSize = (int) dataPageSize.GetValue(model, null);
            }
            else
            {
                _option.EnablePaging = false;
            }

            var haveAccessTo = modelType.GetProperty("HaveAccessTo");
            if (haveAccessTo != null)
            {
                var value = haveAccessTo.GetValue(model, null);
                _option.HaveAccessTo = value;
                _option.EnabledHaveAccessTo = true;
            }
        }

        public GridExtension Options(Action<GridDataOptions> build)
        {
            build(new GridDataOptions(option: _option, htmlHelper: _htmlHelper));
            return this;
        }

        public GridExtension Columns(Action<GridDataColumns> build)
        {
            build(new GridDataColumns(option: _option, htmlHelper: _htmlHelper));
            return this;
        }

        public GridExtension Footer(Action<GridDataFooter> build)
        {
            build(new GridDataFooter(option: _option, htmlHelper: _htmlHelper));
            return this;
        }

        public MvcHtmlString Done()
        {
            if (_option.HaveAccessTo != null && _option.EnabledHaveAccessTo && _option.HaveAccessToRoleAccess == null)
            {
                Options(i =>
                {
                    i.SetHaveAccessTo(_option.HaveAccessTo);
                });
            }

            _option.ListOfData = _option.ListOfData ?? new List<object>();

            if (!_option.ListOfData.Any())
            {
                _option.HideHeaderTag = true;
            }

            var table = DoneTable();

            var panel = DonePanel(table);

            var html = _option.Form != null && _option.Form.IsSetForm ? DoneForm(panel) : panel;

            if (_option.EnablePaging)
            {
                html += ListFooter();
            }

            return new MvcHtmlString(html);
        }

        private string DonePanel(string content)
        {
            var panel = new TagBuilder("div");
            panel.AddCssClass($"panel panel-{_option.TableColor.GetDisplayName()}");

            var rnd = new Random();
            _option.TableLoadingRandom = rnd.Next(1, 999999).ToString();

            var panelHeading = new TagBuilder("div");
            panelHeading.AddCssClass("panel-heading");
            panelHeading.InnerHtml = _option.TableTitle + "<i id=\"tableLoading" + _option.TableLoadingRandom + "\" class=\"fa fa-refresh fa-spin\" style=\"margin-right:10px; display:none;\"></i>";

            var panelFooter = string.Empty;
            if (_option.Form != null && _option.Form.IsSetForm)
            {
                var divPanelFooter = new TagBuilder("div");
                divPanelFooter.AddCssClass("panel-footer");

                var input = new TagBuilder("input");
                input.AddCssClass($"btn btn-{_option.Form.SubmitColor.GetDisplayName()}");
                input.MergeAttribute("type", "submit");
                input.MergeAttribute("value", _option.Form.SubmitText);

                divPanelFooter.InnerHtml = input.ToString();

                panelFooter = divPanelFooter.ToString();
            }

            panel.InnerHtml += panelHeading + content + panelFooter;

            return panel.ToString();
        }

        private string DoneForm(string content)
        {
            if (!_option.Form.IsSetForm)
                return string.Empty;

            var haveAccessTo = true;
            if (_option.EnabledHaveAccessTo)
            {
                haveAccessTo = _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                    _option.Form.HaveAccessToArea, _option.Form.HaveAccessToController,
                    _option.Form.HaveAccessToAction);
            }
            if (!haveAccessTo)
                return string.Empty;

            var form = new TagBuilder("form") { InnerHtml = "" };
            form.MergeAttribute("action", _option.Form.Url);
            form.MergeAttribute("data-ajax", "true");
            if (!string.IsNullOrWhiteSpace(_option.Form.UpdateTarget))
            {
                form.MergeAttribute("data-ajax-update", "#" + _option.Form.UpdateTarget);
            }
            form.MergeAttribute("data-ajax-begin", _option.Form.OnBegin);
            form.MergeAttribute("data-ajax-complete", _option.Form.OnComplete);
            form.MergeAttribute("data-ajax-failure", _option.Form.OnFailure);
            form.MergeAttribute("data-ajax-success", _option.Form.OnSuccess);
            form.MergeAttribute("data-ajax-url", _option.Form.Url);
            form.MergeAttribute("data-ajax-loading", "#tableLoading" + _option.TableLoadingRandom);
            form.MergeAttribute("data-ajax-method", "post");
            form.MergeAttribute("method", "post");
            if (!string.IsNullOrWhiteSpace(_option.Form.GoToPage))
            {
                form.MergeAttribute("data-gotopage", _option.Form.GoToPage);
                form.MergeAttribute("onsubmit", "formonsubmit(this)");
            }

            form.InnerHtml += _htmlHelper.AntiForgeryToken();

            if (_option.Form.HiddenInput != null && _option.Form.HiddenInput.Any())
            {
                foreach (var item in _option.Form.HiddenInput)
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
            fieldset.InnerHtml = content;

            if (_option.Form.IsComplex)
            {
                foreach (var item in _option.HiddenInputKeys)
                {
                    var input = new TagBuilder("input");
                    input.MergeAttribute("type", "hidden");
                    input.MergeAttribute("value", item);
                    input.MergeAttribute("name", $"{_option.Form.Prefix}.Index");

                    fieldset.InnerHtml += input.ToString();
                }
            }

            form.InnerHtml += fieldset.ToString();

            return form.ToString();
        }

        private string DoneTable()
        {
            var divTableResponsive = new TagBuilder("div") { InnerHtml = "" };
            divTableResponsive.AddCssClass("table-responsive");
            divTableResponsive.MergeAttribute("style", "margin: 0 0 0 -1px;");

            var table = new TagBuilder("table");
            table.AddCssClass("table table-hover table-striped well well-white");

            #region Table Head

            var tableTheadTag = string.Empty;

            if (_option.HideHeaderTag == false)
            {
                var tableThead = new TagBuilder("thead");

                var tableTheadTr = new TagBuilder("tr") { InnerHtml = "" };

                foreach (var item in _option.GridColumns)
                {
                    var th = new TagBuilder("th");
                    th.SetInnerText(item.Label);
                    if (item.IsHiddenInput)
                    {
                        th.AddCssClass("display-none");
                    }
                    tableTheadTr.InnerHtml += th.ToString();
                }

                tableThead.InnerHtml = tableTheadTr.ToString();

                tableTheadTag = tableThead.ToString();
            }

            #endregion

            var tableTbody = new TagBuilder("tbody") { InnerHtml = "" };

            _option.PlusPlusNConter = (_option.PageNumber - 1) * _option.PageSize;

            foreach (var data in _option.ListOfData)
            {
                var dataType = data.GetType();

                object id = 0;
                var dataId = dataType.GetProperty("Id");
                if (dataId != null)
                    id = dataId.GetValue(data, null);

                var tr = new TagBuilder("tr") { InnerHtml = "" };
                tr.MergeAttribute("data-id", id.ToString());

                #region Table Colume

                var model = dataType.GetProperties();

                _option.HiddenInputKey = Guid.NewGuid().ToString();
                _option.HiddenInputKeys.Add(_option.HiddenInputKey);

                foreach (var item in _option.GridColumns)
                {

                    var td = new TagBuilder("td");

                    if (item.IsHiddenInput)
                    {
                        td.AddCssClass("display-none");
                    }

                    if (item.IsColumnCounter)
                    {
                        var plus = ++_option.PlusPlusNConter;

                        if (item.IsCustomTemplate)
                        {
                            td.InnerHtml += string.Format(item.IsCustomTemplateHtml, plus);
                        }
                        else
                        {
                            td.SetInnerText(plus.ToString());
                        }

                        tr.InnerHtml += td.ToString();
                        continue;
                    }

                    if (item.IsColumnForButton)
                    {
                        td.MergeAttribute("class","columnbutton");
                        if (_option.Buttons.Any())
                        {
                            foreach (var button in _option.Buttons)
                            {
                                var tag = DoneButton(button, data, dataType);
                                td.InnerHtml += tag;
                            }

                            tr.InnerHtml += td.ToString();
                            continue;
                        }
                    }

                    if (item.IsHtml && item.IsHtmlTemplate.UsePropertyValue == false)
                    {
                        td.InnerHtml += DoneHtml(column: item, option: _option, value: null);
                        tr.InnerHtml += td.ToString();
                        continue;
                    }

                    var firstOrDefault = model.FirstOrDefault(x => x.Name == item.PropertyName);
                    if (firstOrDefault != null)
                    {
                        var propertValue = firstOrDefault.GetValue(data, null);

                        if (item.IsFinancial)
                        {
                            var isFinancial = long.Parse(propertValue.ToString()).ToString("#,##0");
                            if (item.IsFinancialWithRialExt)
                            {
                                isFinancial = $"{isFinancial} ریال";
                            }
                            propertValue = isFinancial;
                        }

                        if (_option.IsRowColorRoles && !string.IsNullOrWhiteSpace(_option.RowColorRolesPropertyName))
                        {
                            if (item.PropertyName == _option.RowColorRolesPropertyName)
                            {
                                if (propertValue != null && _option.RowColorRoles.Any(x => x.Key == propertValue.ToString()))
                                {
                                    var value = _option.RowColorRoles
                                        .FirstOrDefault(x => x.Key == propertValue.ToString())
                                        .Value;
                                    tr.MergeAttribute("class", value.GetDisplayName());
                                }
                            }
                        }

                        if (item.IsHtml && item.IsHtmlTemplate.UsePropertyValue)
                        {
                            td.InnerHtml += DoneHtml(column: item, option: _option, value: propertValue);
                            tr.InnerHtml += td.ToString();
                            continue;
                        }

                        if (item.IsList == false || string.IsNullOrWhiteSpace(item.IsListPropertyName))
                        {
                            propertValue = propertValue?.ToString() ?? string.Empty;
                            propertValue = PropertValue(firstOrDefault, propertValue);

                            if (item.IsReplaceValueWith)
                            {
                                if (item.ReplaceValueWiths != null && item.ReplaceValueWiths.Any(x=>x.Key == (string) propertValue))
                                {
                                    propertValue = item.ReplaceValueWiths.First(x => x.Key == (string) propertValue).Value;
                                }
                            }

                            td.SetInnerText(propertValue.ToString());

                            if (item.IsBoolean)
                            {
                                if (propertValue as string != "-")
                                {
                                    var boolValue = bool.Parse(propertValue.ToString());
                                    propertValue = boolValue ? item.IsBooleanOnTrueTemplate : item.IsBooleanOnFalseTemplate;
                                }
                                td.InnerHtml = propertValue.ToString();
                            }

                            if (item.IsCustomTemplate)
                            {
                                propertValue = string.Format(item.IsCustomTemplateHtml, propertValue);
                                td.InnerHtml = propertValue.ToString();
                            }
                        }
                        else
                        {
                            var text = string.Empty;
                            var list = (IList)propertValue;
                            if (list != null)
                            {
                                foreach (var l in list)
                                {
                                    var propertyInfo = l.GetType().GetProperty(item.IsListPropertyName);
                                    if (propertyInfo != null)
                                    {
                                        var value = propertyInfo.GetValue(l, null);
                                        if (!string.IsNullOrWhiteSpace(item.IsCustomTemplateHtml))
                                        {
                                            text += string.Format(item.IsCustomTemplateHtml, value);
                                        }
                                        else
                                        {
                                            text += $" {value} ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                text = "-";
                            }

                            td.InnerHtml = text;
                        }

                        tr.InnerHtml += td.ToString();
                    }
                }

                #endregion

                tableTbody.InnerHtml += tr.ToString();
            }

            if (_option.ShowEmptyResult && !_option.ListOfData.Any())
            {
                var span = new TagBuilder("span");
                span.SetInnerText(!string.IsNullOrWhiteSpace(_option.EmptyResultMessage) ? _option.EmptyResultMessage : "اطلاعاتی برای نمایش وجود ندارد.");

                var div = new TagBuilder("div");
                div.AddCssClass("alert alert-warning text-center margin-0");
                div.InnerHtml = span.ToString();

                var td = new TagBuilder("td");
                td.MergeAttribute("colspan", _option.GridColumns.Count.ToString());
                td.InnerHtml = div.ToString();

                var tr = new TagBuilder("tr") {InnerHtml = td.ToString()};

                tableTbody.InnerHtml += tr.ToString();
            }

            table.InnerHtml += tableTheadTag + tableTbody;

            #region Footer

            if (_option.IsSetTableFooter)
            {
                var tableFoot = new TagBuilder("tfoot");

                if (_option.HideFooterIfEmpty && !_option.ListOfData.Any())
                {
                    tableFoot.AddCssClass("display-none");
                }

                var tableFootTr = new TagBuilder("tr");

                var tableFootTd = new TagBuilder("td");
                tableFootTd.MergeAttribute("colspan", _option.GridColumns.Count.ToString());

                foreach (var button in _option.FooterButtons)
                {
                    tableFootTd.InnerHtml += DoneButtonForFooter(button);
                }

                foreach (var footerItem in _option.GridFooter)
                {
                    tableFootTd.InnerHtml += DoneHtmlFooter(footerItem, _option);
                }

                tableFootTr.InnerHtml += tableFootTd.ToString();

                tableFoot.InnerHtml += tableFootTr.ToString();

                table.InnerHtml += tableFoot.ToString();
            }

            #endregion

            divTableResponsive.InnerHtml += table.ToString();

            return divTableResponsive.ToString();
        }

        private string DoneButton(GridButton button, object data, Type dataType)
        {
            var haveAccessToButton = true;
            if (_option.EnabledHaveAccessTo)
                haveAccessToButton =
                    _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                    button.HaveAccessToArea, button.HaveAccessToController, button.HaveAccessToAction);

            if (string.IsNullOrWhiteSpace(button.HaveAccessToArea) || string.IsNullOrWhiteSpace(button.HaveAccessToController) ||
                string.IsNullOrWhiteSpace(button.HaveAccessToAction))
                haveAccessToButton = true;

            if (!haveAccessToButton)
                return string.Empty;

            if (button.ShowIf)
            {
                var dataIfTrue = dataType.GetProperty(button.ShowIfPropertyName);
                if (dataIfTrue != null)
                {
                    var dataValue = dataIfTrue.GetValue(data, null);
                    if (dataValue?.ToString().Equals(button.ShowIfValue) == false)
                    {
                        return string.Empty;
                    }
                }
            }

            if (button.HideIf)
            {
                var dataIfTrue = dataType.GetProperty(button.HideIfPropertyName);
                if (dataIfTrue != null)
                {
                    var dataValue = dataIfTrue.GetValue(data, null);
                    if (dataValue?.ToString().Equals(button.HideIfValue) == true)
                    {
                        return string.Empty;
                    }
                }
            }

            var tag = new TagBuilder(button.TagType);

            var queryStringWithValue = new Dictionary<string, string>();
            if (button.QureyStrings.Any())
            {
                foreach (var qureyString in button.QureyStrings)
                {
                    if (!qureyString.IsWithValue)
                    {
                        var dataQueryString = dataType.GetProperty(qureyString.Value);
                        if (dataQueryString != null)
                        {
                            var dataValue = dataQueryString.GetValue(data, null);
                            if (dataValue != null)
                            {
                                queryStringWithValue.Add(qureyString.Key, dataValue.ToString());
                            }
                        }
                    }
                    else
                    {
                        queryStringWithValue.Add(qureyString.Key, qureyString.Value);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(button.Href))
            {
                var href = button.Href;
                if (queryStringWithValue.Any())
                    if (queryStringWithValue.Count == 1 && queryStringWithValue.ContainsKey("id"))
                    {
                        href = $"{button.Href}/{queryStringWithValue.First().Value}";
                    }
                    else
                    {
                        var values = queryStringWithValue.Select(x => $"{x.Key}={x.Value}").ToList();
                        href = button.Href + "?" + string.Join("&", values);
                    }

                if (button.HtmlAttributes.ContainsKey("data-ajax-url"))
                {
                    button.HtmlAttributes["data-ajax-url"] = href;
                }
                else if (button.HtmlAttributes.ContainsKey("data-popup"))
                {
                    button.HtmlAttributes["data-url"] = href;
                }
                else
                {
                    tag.MergeAttribute("href", href);
                }
            }

            foreach (var htmlAttribute in button.HtmlAttributes)
            {
                if (!string.IsNullOrWhiteSpace(htmlAttribute.Value) && Regex.IsMatch(htmlAttribute.Value, "\\[\\w+\\]"))
                {
                    var v = string.Empty;

                    var matchs = Regex.Match(htmlAttribute.Value, "\\[\\w+\\]");
                    foreach (var match in matchs.Groups)
                    {
                        object propertyValue = null;
                        var name = match.ToString().Trim(Convert.ToChar("["), Convert.ToChar("]"));
                        var propertyInfo = dataType.GetProperty(name);
                        if (propertyInfo != null)
                            propertyValue = propertyInfo.GetValue(data, null);

                        if (propertyValue != null)
                        {
                            v += htmlAttribute.Value.Replace(match.ToString(), propertyValue.ToString());
                        }
                    }

                    tag.MergeAttribute(htmlAttribute.Key, v);
                }
                else
                {
                    tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(button.IconClass))
            {
                var i = new TagBuilder("i");
                i.AddCssClass(button.IconClass);
                i.MergeAttribute("style", "margin-left:2px;");

                tag.InnerHtml += i + button.Text;
            }
            else
            {
                tag.SetInnerText(button.Text);
            }

            if (!string.IsNullOrWhiteSpace(button.GoToPage))
            {
                tag.MergeAttribute("data-gotopage", button.GoToPage);
                tag.MergeAttribute("onclick", "formonsubmit(this)");
            }

            return tag.ToString();
        }

        private string DoneButtonForFooter(GridButton button)
        {
            var haveAccessToButton = true;
            if (_option.EnabledHaveAccessTo)
                haveAccessToButton =
                    _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                        button.HaveAccessToArea, button.HaveAccessToController, button.HaveAccessToAction);

            if (string.IsNullOrWhiteSpace(button.HaveAccessToArea) || string.IsNullOrWhiteSpace(button.HaveAccessToController) ||
                string.IsNullOrWhiteSpace(button.HaveAccessToAction))
                haveAccessToButton = true;

            if (!haveAccessToButton)
                return string.Empty;

            var tag = new TagBuilder(button.TagType);

            var queryStringWithValue = new Dictionary<string, string>();
            if (button.QureyStrings.Any(x=>x.IsWithValue))
            {
                foreach (var qureyString in button.QureyStrings)
                {
                    queryStringWithValue.Add(qureyString.Key, qureyString.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(button.Href))
            {
                var href = button.Href;
                if (queryStringWithValue.Any())
                    if (queryStringWithValue.Count == 1 && queryStringWithValue.ContainsKey("id"))
                    {
                        href = $"{button.Href}/{queryStringWithValue.First().Value}";
                    }
                    else
                    {
                        var values = queryStringWithValue.Select(x => $"{x.Key}={x.Value}").ToList();
                        href = button.Href + "?" + string.Join("&", values);
                    }

                if (button.HtmlAttributes.ContainsKey("data-ajax-url"))
                {
                    button.HtmlAttributes["data-ajax-url"] = href;
                }
                else if (button.HtmlAttributes.ContainsKey("data-popup"))
                {
                    button.HtmlAttributes["data-url"] = href;
                }
                else
                {
                    tag.MergeAttribute("href", href);
                }
            }

            foreach (var htmlAttribute in button.HtmlAttributes)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (!string.IsNullOrWhiteSpace(button.IconClass))
            {
                var i = new TagBuilder("i");
                i.AddCssClass(button.IconClass);
                i.MergeAttribute("style", "margin-left:2px;");

                tag.InnerHtml += i + button.Text;
            }
            else
            {
                tag.SetInnerText(button.Text);
            }

            if (!string.IsNullOrWhiteSpace(button.GoToPage))
            {
                tag.MergeAttribute("data-gotopage", button.GoToPage);
                tag.MergeAttribute("onclick", "formonsubmit(this)");
            }

            return tag.ToString();
        }

        private static string DoneHtml(GridColumn column, GridOption option, object value)
        {

            var tag = new TagBuilder(column.IsHtmlTemplate.TagType);

            foreach (var htmlAttribute in column.IsHtmlTemplate.HtmlAttributes)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (column.IsHtmlTemplate.TagType == "input")
            {
                tag.MergeAttribute("value", value?.ToString() ?? string.Empty);
                if (tag.Attributes.ContainsKey("type") && tag.Attributes["type"] == "checkbox")
                {
                    if (!string.IsNullOrWhiteSpace(column.IsHtmlTemplate.Value) && value?.ToString() == column.IsHtmlTemplate.Value)
                    {
                        tag.MergeAttribute("checked", "checked");
                    }
                }
            }
            else if (column.IsHtmlTemplate.TagType == "select")
            {
                var chooseSelect = new TagBuilder("option");
                chooseSelect.SetInnerText("انتخاب کنید");
                tag.InnerHtml += chooseSelect.ToString();

                if (column.IsHtmlTemplate.Child.Any())
                {
                    foreach (var item in column.IsHtmlTemplate.Child)
                    {
                        var oSelect = new TagBuilder("option");
                        oSelect.MergeAttribute("value", item.Key);
                        oSelect.SetInnerText(item.Value);
                        if (value != null && item.Key == value.ToString())
                        {
                            oSelect.MergeAttribute("selected", "selected");
                        }

                        tag.InnerHtml += oSelect.ToString();
                    }
                }
            }

            var name = column.IsHtmlTemplate.Name;
            if (option.Form != null && option.Form.IsComplex)
            {
                name = $"{option.Form.Prefix}[{option.HiddenInputKey}].{column.IsHtmlTemplate.Name}";
            }

            tag.MergeAttribute("name", name);

            if (option.Form != null && option.Form.IsComplex)
            {
                tag.MergeAttribute("id", name.GetIdFromName());
            }

            var html = tag.ToString();

            if (column.IsHtmlTemplate.Validation)
            {
                var span = new TagBuilder("span");
                span.MergeAttribute("class", "field-validation-valid text-danger");
                span.MergeAttribute("data-valmsg-for", name);
                span.MergeAttribute("data-valmsg-replace", "true");

                html += span.ToString();
            }

            return html;
        }

        private static string DoneHtmlFooter(GridColumn column, GridOption option)
        {
            var html = string.Empty;

            if (!string.IsNullOrWhiteSpace(column.Label))
            {
                var label = new TagBuilder("label");
                label.AddCssClass("control-label");
                label.SetInnerText(column.Label);

                html += label.ToString();
            }

            var tag = new TagBuilder(column.IsHtmlTemplate.TagType);

            foreach (var htmlAttribute in column.IsHtmlTemplate.HtmlAttributes)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (column.IsHtmlTemplate.TagType == "input")
            {
            }
            else if (column.IsHtmlTemplate.TagType == "select")
            {
                var chooseSelect = new TagBuilder("option");
                chooseSelect.SetInnerText("انتخاب کنید");
                tag.InnerHtml += chooseSelect.ToString();

                if (column.IsHtmlTemplate.Child.Any())
                {
                    foreach (var item in column.IsHtmlTemplate.Child)
                    {
                        var oSelect = new TagBuilder("option");
                        oSelect.MergeAttribute("value", item.Key);
                        oSelect.SetInnerText(item.Value);
                        if (item.Key == column.IsHtmlTemplate.Value)
                        {
                            oSelect.MergeAttribute("selected", "selected");
                        }
                        tag.InnerHtml += oSelect.ToString();
                    }
                }
            }

            var name = column.IsHtmlTemplate.Name;
            if (option.Form != null && option.Form.IsComplex)
            {
                name = $"{option.Form.Prefix}[{option.HiddenInputKey}].{column.IsHtmlTemplate.Name}";
            }

            tag.MergeAttribute("name", name);
            tag.MergeAttribute("id", name.GetIdFromName());

            if (column.IsHtmlTemplate.HtmlAttributes.ContainsKey("data-mddatetimepicker"))
            {
                tag.MergeAttribute("data-targetselector", "#" + name.GetIdFromName());
            }

            html += tag.ToString();

            if (column.IsHtmlTemplate.Validation)
            {
                var span = new TagBuilder("span");
                span.MergeAttribute("class", "field-validation-valid text-danger");
                span.MergeAttribute("data-valmsg-for", name);
                span.MergeAttribute("data-valmsg-replace", "true");

                html += span.ToString();
            }

            var div = new TagBuilder("div");
            div.AddCssClass($"col-sm-{column.IsHtmlTemplate.ColSize}");

            div.InnerHtml = html;

            return div.ToString();
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
                        propertValue = d.ToShortShamsi();
                    }
                    else
                    {
                        propertValue = "-";
                    }
                }
            }

            return string.IsNullOrWhiteSpace(propertValue.ToString()) ? "-" : propertValue;
        }

        private static MvcHtmlString ListFooter()
        {
            var divCol12 = new TagBuilder("div");
            divCol12.AddCssClass("row");

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var controller = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString().ToLower();
            var action = _htmlHelper.ViewContext.RouteData.Values["action"].ToString().ToLower();

            var requestQueryString = _htmlHelper.ViewContext.HttpContext.Request.QueryString;
            var getCollection = new FormCollection(requestQueryString);

                var areaName = "";
            if (_htmlHelper.ViewContext.RouteData.DataTokens.ContainsKey("area"))
            {
                 areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            }

            var queryStringPageNumber = new RouteValueDictionary() { { "area", areaName } };
            foreach (string item in getCollection)
            {
                queryStringPageNumber.Add(item.ToLower(), requestQueryString.Get(item.ToLower()));
            }

            if (queryStringPageNumber.All(x => x.Key != "pagesize"))
            {
                queryStringPageNumber.Add("pagesize", _option.PageSize);
            }
            else
            {
                queryStringPageNumber["pageSize"] = _option.PageSize;
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
                queryStringPaging.Add("pagesize", _option.PageSize);
            }

            var divCol8 = new TagBuilder("div");
            divCol8.AddCssClass("col-md-8 text-right-rtl");
            divCol8.InnerHtml = _htmlHelper.Pager(pageSize: _option.PageSize, currentPage: _option.PageNumber, totalItemCount: _option.PageTotal).Options(o => o
                .DisplayTemplate("Bootstrap3Pagination").RouteValues(queryStringPaging)).ToHtmlString();

            divCol12.InnerHtml += divCol8;

            if (_option.EnablePageSize)
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
                    if (_option.PageSize == item.Key)
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
}