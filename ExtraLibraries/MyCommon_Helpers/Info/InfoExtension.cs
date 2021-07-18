using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Info
{
    public class InfoExtension
    {
        private static HtmlHelper _htmlHelper;
        private static InfoOption _option;

        public InfoExtension(HtmlHelper htmlHelper, object model)
        {
            _htmlHelper = htmlHelper;
            var modelType = model.GetType();

            _option = new InfoOption()
            {
                RequestContext = htmlHelper.ViewContext.RequestContext,
            };

            var haveAccessTo = modelType.GetProperty("HaveAccessTo");
            if (haveAccessTo != null)
            {
                var value = haveAccessTo.GetValue(model, null);
                _option.HaveAccessTo = value;
                _option.EnabledHaveAccessTo = true;
            }
        }

        public InfoExtension Options(Action<InfoDataOption> buildAction)
        {
            buildAction(new InfoDataOption(options: _option));
            return this;
        }

        public InfoExtension Items(Action<InfoDataItem> buildAction)
        {
            buildAction(new InfoDataItem(options: _option));
            return this;
        }

        public InfoExtension Footers(Action<InfoDataFooter> buildAction)
        {
            buildAction(new InfoDataFooter(options: _option, htmlHelper: _htmlHelper));
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

            foreach (var a in _option.InfoItems)
            {
                if (a.ColSize != null)
                    continue;

                if (_option.Colume == EInfoColumeType.Column6)
                {
                    a.ColSize = (int)EInfoColumeType.Column6;
                }
                else if (_option.Colume == EInfoColumeType.Column4)
                {
                    a.ColSize = (int)EInfoColumeType.Column4;
                }
                else if (_option.Colume == EInfoColumeType.Column3)
                {
                    a.ColSize = (int)EInfoColumeType.Column3;
                }
                else if (_option.Colume == EInfoColumeType.Column2)
                {
                    a.ColSize = (int)EInfoColumeType.Column2;
                }
                else if (_option.Colume == EInfoColumeType.Column1)
                {
                    a.ColSize = (int)EInfoColumeType.Column1;
                }
            }

            var listOfDefaultInfoForItem = new List<List<InfoItem>>();
            var secondList = new List<InfoItem>();
            var colume = 0;
            foreach (var item in _option.InfoItems)
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
                if (_option.Colume == EInfoColumeType.Column2)
                {
                    if (item.Count != 2)
                    {
                        item.Last().Colspan = 2 - item.Count + 1;
                        continue;
                    }
                }
                if (_option.Colume == EInfoColumeType.Column3)
                {
                    if (item.Count != 3)
                    {
                        item.Last().Colspan = 3 - item.Count + 1;
                        continue;
                    }
                }
                if (_option.Colume == EInfoColumeType.Column4)
                {
                    if (item.Count != 4)
                    {
                        item.Last().Colspan = 4 - item.Count + 1;
                        continue;
                    }
                }
                if (_option.Colume == EInfoColumeType.Column6)
                {
                    if (item.Count != 6)
                    {
                        item.Last().Colspan = 6 - item.Count + 1;
                    }
                }
            }

            var content = string.Empty;

            foreach (var rowItems in listOfDefaultInfoForItem)
            {
                var tr = new TagBuilder("tr") { InnerHtml = "" };

                foreach (var item in rowItems)
                {
                    tr.InnerHtml += DoneTableTrTd(item);
                }

                content += tr.ToString();
            }

            var html = DonePanel(content);

            return new MvcHtmlString(html);
        }

        private string DoneButton(InfoButton button)
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

            var tag = new TagBuilder("a");

            var queryStringWithValue = new Dictionary<string, string>();
            if (button.QureyStrings.Any())
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

        private static object PropertValue(PropertyInfo propertyInfo, object propertValue)
        {
            if (propertyInfo == null)
            {
                
            }
            else if (propertyInfo.PropertyType.IsNullableEnum())
            {
                var answer = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                if (answer != null)
                {
                    foreach (var fieldInfo in answer.GetFields())
                    {
                        if (fieldInfo.Name == propertValue?.ToString())
                        {
                            propertValue = fieldInfo.GetCustomAttribute<DisplayAttribute>().Name;
                        }
                    }
                }
                else
                {
                    foreach (var fieldInfo in propertyInfo.PropertyType.GetFields())
                    {
                        if (fieldInfo.Name == propertValue.ToString())
                        {
                            propertValue = fieldInfo.GetCustomAttribute<DisplayAttribute>().Name;
                        }
                    }
                }
            }
            else if (propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime))
            {
                if (!string.IsNullOrWhiteSpace(propertValue?.ToString()))
                {
                    var d = DateTime.Parse(propertValue.ToString());
                    if (d.Year != 0001)
                    {
                        propertValue = d.ToShortShamsi();
                    }
                }
            }

            return string.IsNullOrWhiteSpace(propertValue?.ToString()) ? "-" : propertValue;
        }

        private string DoneTableTrTd(InfoItem item)
        {
            var span = new TagBuilder("span");

            if (item.PopOver)
            {
                span.MergeAttribute("tabindex", "0");
                span.MergeAttribute("role", "button");
                span.MergeAttribute("data-toggle", "popover");
                span.MergeAttribute("data-trigger", "focus");
                span.MergeAttribute("data-placement", item.PopOverPlacement);
                span.MergeAttribute("title", item.PopOverTitle);
                if (item.PopOverLtr)
                {
                    span.MergeAttribute("data-content",
                        "<div style='direction:ltr;text-align:left;'>" + item.PopOverContent + "</div>");
                }
                else
                {
                    span.MergeAttribute("data-content", item.PopOverContent);
                }
                span.MergeAttribute("style", "cursor:pointer;");
            }

            if (item.IsBoolean)
            {
                if (!string.IsNullOrWhiteSpace(item.Value.ToString()))
                {
                    if (item.IsCustomTemplate)
                    {
                        span.InnerHtml = bool.Parse(item.Value.ToString())
                            ? string.Format(item.IsCustomTemplateHtml, item.IsBooleanOnTrueTemplate) 
                            : string.Format(item.IsCustomTemplateHtml, item.IsBooleanOnFalseTemplate);
                    }
                    else
                    {
                        span.InnerHtml = bool.Parse(item.Value.ToString())
                            ? item.IsBooleanOnTrueTemplate
                            : item.IsBooleanOnFalseTemplate;
                    }
                }
            }

            else if (item.IsList && item.Value != null)
            {
                var list = (IList) item.Value;
                foreach (var l in list)
                {
                    var memberInfo = l.GetType().GetProperty(item.IsListPropertyName);
                    if (memberInfo != null)
                    {
                        var value = memberInfo.GetValue(l, null);
                        value = PropertValue(memberInfo, value).ToString();
                        if (item.IsCustomTemplate)
                        {
                            span.InnerHtml += string.Format(item.IsCustomTemplateHtml, value);
                        }
                        else
                        {
                            span.InnerHtml += $" {value} ";
                        }
                    }
                }
            }

            else
            {
                var v = PropertValue(item.PropertyInfo, item.Value).ToString();
                if (item.IsReplaceValueWith && item.ReplaceValueWiths.Any(x=>x.Key == v))
                {
                    v = item.ReplaceValueWiths.First(x => x.Key == v).Value;
                }
                else
                if (item.IsFinancial)
                {
                    var isFinancial = long.Parse(v).ToString("#,##0");
                    if (item.IsFinancialWithRialExt)
                    {
                        isFinancial = $"{isFinancial} ریال";
                    }
                    v = isFinancial;
                }

                span.InnerHtml = item.IsCustomTemplate ? string.Format(item.IsCustomTemplateHtml, v) : v;
            }

            if (string.IsNullOrWhiteSpace(span.InnerHtml))
                span.InnerHtml = "-";

            var labelStr = string.Empty;
            if (!string.IsNullOrWhiteSpace(item.Label))
            {
                var label = new TagBuilder("label");
                label.AddCssClass("small" + (!string.IsNullOrWhiteSpace(_option.Labelclass) ? $" {_option.Labelclass}" : ""));
                label.SetInnerText(!string.IsNullOrWhiteSpace(item.Label) ? item.Label : "*");

                labelStr = $"{label}: ";
            }

            var td = new TagBuilder("td") { InnerHtml = "" };
            if (item.Colspan != null)
            {
                td.MergeAttribute("colspan", item.Colspan.ToString());
            }
            if (item.IsEmptyLine)
            {
                td.MergeAttribute("style", "padding:0;");
            }
            if (item.IsTitle)
            {
                var tagH4 = new TagBuilder("h4") {InnerHtml = span.ToString()};
                tagH4.MergeAttribute("style", "margin:15px 0 0 0;");

                td.InnerHtml += tagH4.ToString();
            }

            if (!item.IsEmptyLine && !item.IsTitle)
            {
                td.InnerHtml += labelStr + span;
            }

            return td.ToString();
        }

        private string DonePanel(string content)
        {
            var divPanel = new TagBuilder("div") { InnerHtml = "" };
            divPanel.AddCssClass($"panel panel-{_option.Color.GetDisplayName()} defaultInfo {_option.PanelClass}");

            var divPanelHeading = new TagBuilder("div");
            divPanelHeading.AddCssClass("panel-heading");

            var divPanelHeadingH5 = new TagBuilder("h5");
            divPanelHeadingH5.AddCssClass("margin-0");
            divPanelHeadingH5.SetInnerText(_option.Title);

            divPanelHeading.InnerHtml = divPanelHeadingH5.ToString();

            divPanel.InnerHtml += divPanelHeading.ToString();

            var divTableResponsive = new TagBuilder("div");
            divTableResponsive.AddCssClass("table-responsive");

            var table = new TagBuilder("table");
            table.AddCssClass("table");

            var tbody = new TagBuilder("tbody")
            {
                InnerHtml = content
            };

            table.InnerHtml = tbody.ToString();

            divTableResponsive.InnerHtml = table.ToString();

            divPanel.InnerHtml += divTableResponsive.ToString();

            if (_option.EnablePanelFooter)
            {
                var footerHtml = _option.PanelFooterButtons.Aggregate(string.Empty, (current, button) => current + DoneButton(button));
                if (!string.IsNullOrWhiteSpace(footerHtml))
                {
                    var divPanelFooter = new TagBuilder("div");
                    divPanelFooter.AddCssClass("panel-footer");
                    divPanelFooter.InnerHtml = footerHtml;
                    divPanel.InnerHtml += divPanelFooter.ToString();
                }
            }

            return divPanel.ToString();
        }

    }
}