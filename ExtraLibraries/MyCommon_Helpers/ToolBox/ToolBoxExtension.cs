using System;
using System.Linq;
using System.Reflection.Emit;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.ToolBox
{
    public class ToolBoxExtension
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly ToolBoxOption _option;

        public ToolBoxExtension(HtmlHelper htmlHelper, object model)
        {
            _htmlHelper = htmlHelper;
            _option = new ToolBoxOption()
            {
                RequestContext = _htmlHelper.ViewContext.RequestContext,
            };

            var modelType = model.GetType();
            var haveAccessTo = modelType.GetProperty("HaveAccessTo");
            if (haveAccessTo != null)
            {
                var value = haveAccessTo.GetValue(model, null);
                _option.HaveAccessTo = value;
                _option.EnabledHaveAccessTo = true;
            }

        }

        public ToolBoxExtension Options(Action<ToolBoxDataOption> buildOptions)
        {
            buildOptions(new ToolBoxDataOption(options: _option));
            return this;
        }

        public ToolBoxExtension Items(Action<ToolBoxDataItems> buildOptions)
        {
            buildOptions(new ToolBoxDataItems(option: _option, htmlHelper: _htmlHelper));
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

            var html = string.Empty;

            var itemsHtml = string.Empty;
            foreach (var item in _option.Items)
            {
                if (item.IsButton)
                {
                    var haveAccessToButton = true;
                    if (_option.EnabledHaveAccessTo)
                        haveAccessToButton =
                            _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                                item.Button.HaveAccessToArea, item.Button.HaveAccessToController, item.Button.HaveAccessToAction);

                    if (haveAccessToButton)
                    {
                        var tag = item.Button.IsAjaxForm ? DoneAjaxButton(item.Button) : DoneButton(item.Button);
                        itemsHtml += tag;
                    }
                }
                else if (item.IsDataInfo)
                {
                    itemsHtml += DoneDataInfo(item);
                } 
                else if (item.IsRadioGroup)
                {
                    itemsHtml += DoneRadioGroup(item.RadioGroup);
                }
            }

            if (!string.IsNullOrWhiteSpace(itemsHtml))
            {
                var div = new TagBuilder("div");
                div.AddCssClass("well well-shadow");

                if (!string.IsNullOrWhiteSpace(_option.Style))
                {
                    div.MergeAttribute("style",_option.Style);
                }

                div.InnerHtml += itemsHtml;

                html += div.ToString();
            }

            return new MvcHtmlString(html);
        }

        private string DoneButton(ToolBoxButton button)
        {
            var haveAccessToButton = true;
            if (_option.EnabledHaveAccessTo)
                haveAccessToButton =
                    _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                        button.HaveAccessToArea, button.HaveAccessToController, button.HaveAccessToAction);

            if (!haveAccessToButton)
                return string.Empty;

            var tag = new TagBuilder("a");

            if (!button.IsDisabled)
            {
                if (!string.IsNullOrWhiteSpace(button.Href))
                {
                    var href = button.Href;
                    if (button.QureyStrings.Any())
                        if (button.QureyStrings.Count == 1 && button.QureyStrings.ContainsKey("id"))
                        {
                            href = $"{button.Href}/{button.QureyStrings.First().Value}";
                        }
                        else
                        {
                            var values = button.QureyStrings.Select(x => $"{x.Key}={x.Value}").ToList();
                            if (values.Any())
                            {
                                href = button.Href + "?" + string.Join("&", values);
                            }
                            else
                            {
                                href = button.Href;
                            }
                        }

                    tag.MergeAttribute("href", href);
                    if (button.HtmlAttributes.ContainsKey("data-ajax-url"))
                    {
                        button.HtmlAttributes["data-ajax-url"] = href;
                    }
                }
            }
            else
            {
                if (button.HtmlAttributes.ContainsKey("class"))
                {
                    button.HtmlAttributes["class"] = button.HtmlAttributes["class"] + " margin-left-10" + " disabled";
                }
                else
                {
                    button.HtmlAttributes.Add("class", "margin-left-10 disabled");
                }
                tag.MergeAttribute("href", "");
                if (button.HtmlAttributes.ContainsKey("data-ajax-url"))
                {
                    button.HtmlAttributes["data-ajax-url"] = "";
                }
            }

            if (button.HtmlAttributes.ContainsKey("data-ajax"))
            {
                var numRnd = Guid.NewGuid().ToString().Substring(0, 8);
                button.HtmlAttributes["data-ajax-loading"] = $"#loading{numRnd}";
            }
            if (button.HtmlAttributes.ContainsKey("data-url"))
            {
                var values = button.QureyStrings.Select(x => $"{x.Key}={x.Value}").ToList();
                if (values.Any())
                {
                    button.HtmlAttributes["data-url"] = button.HtmlAttributes["data-url"] + "?" +
                                                        string.Join("&", values);
                }
            }

            foreach (var htmlAttribute in button.HtmlAttributes)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            foreach (var htmlAttribute in button.HtmlAttributesForLink)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (!string.IsNullOrWhiteSpace(button.IconClass) || button.HtmlAttributes.ContainsKey("data-ajax"))
            {
                if (button.HtmlAttributes.ContainsKey("data-ajax"))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass("fa fa-refresh fa-spin");
                    i.MergeAttribute("style", "margin-left:2px; margin-right:2px; display:none;");
                    i.MergeAttribute("id", button.HtmlAttributes["data-ajax-loading"].Substring(1));

                    tag.InnerHtml += i.ToString();
                }

                if (!string.IsNullOrWhiteSpace(button.IconClass))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass(button.IconClass);
                    i.MergeAttribute("style", "margin-left:2px;");

                    tag.InnerHtml += i.ToString();
                }

                tag.InnerHtml += button.Text;
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

        private string DoneAjaxButton(ToolBoxButton button)
        {
            var numRnd = Guid.NewGuid().ToString().Substring(0, 8);

            var haveAccessToButton = true;
            if (_option.EnabledHaveAccessTo)
                haveAccessToButton =
                    _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                        button.HaveAccessToArea, button.HaveAccessToController, button.HaveAccessToAction);

            if (!haveAccessToButton)
                return string.Empty;

            var form = new TagBuilder("form") { InnerHtml = "" };
            form.MergeAttribute("id", $"form{numRnd}");
            form.MergeAttribute("action", button.Href);
            form.MergeAttribute("data-ajax", "true");
            form.MergeAttribute("data-ajax-begin", button.HtmlAttributes["data-ajax-begin"]);
            form.MergeAttribute("data-ajax-complete", button.HtmlAttributes["data-ajax-complete"]);
            form.MergeAttribute("data-ajax-failure", button.HtmlAttributes["data-ajax-failure"]);
            form.MergeAttribute("data-ajax-success", button.HtmlAttributes["data-ajax-success"]);
            form.MergeAttribute("data-ajax-url", button.Href);
            form.MergeAttribute("data-ajax-loading", button.HtmlAttributes["data-ajax-loading"] + numRnd);
            form.MergeAttribute("data-ajax-method", "post");
            form.MergeAttribute("method", "post");
            form.MergeAttribute("style", "display:none;");
            if (!string.IsNullOrWhiteSpace(button.GoToPage))
            {
                form.MergeAttribute("data-gotopage", button.GoToPage);
                form.MergeAttribute("onsubmit", "formonsubmit(this)");
            }

            form.InnerHtml += _htmlHelper.AntiForgeryToken();

            if (button.QureyStrings != null && button.QureyStrings.Any())
            {
                foreach (var item in button.QureyStrings)
                {
                    var input = new TagBuilder("input");
                    input.MergeAttribute("type", "hidden");
                    input.MergeAttribute("name", item.Key);
                    input.MergeAttribute("value", item.Value);

                    form.InnerHtml += input.ToString();
                }
            }

            var a = new TagBuilder("a");
            if (button.HtmlAttributes.ContainsKey("class"))
            {
                a.AddCssClass(button.HtmlAttributes["class"] + " margin-left-10" + (button.IsDisabled ? " disabled" : ""));
            }
            else
            {
                a.AddCssClass("margin-left-10" + (button.IsDisabled ? "disabled" : ""));
            }
            if (button.HtmlAttributes.ContainsKey("style"))
            {
                a.MergeAttribute("style", button.HtmlAttributes["style"]);
            }

            a.MergeAttribute("data-form", "true");
            a.MergeAttribute("data-form-id", $"form{numRnd}");

            foreach (var htmlAttribute in button.HtmlAttributesForLink)
            {
                a.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (!string.IsNullOrWhiteSpace(button.IconClass) || button.HtmlAttributes.ContainsKey("data-ajax"))
            {
                if (button.HtmlAttributes.ContainsKey("data-ajax"))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass("fa fa-refresh fa-spin");
                    i.MergeAttribute("style", "margin-left:2px; margin-right:2px; display:none;");
                    i.MergeAttribute("id", button.HtmlAttributes["data-ajax-loading"].Substring(1) + numRnd);

                    a.InnerHtml += i.ToString();
                }

                if (!string.IsNullOrWhiteSpace(button.IconClass))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass(button.IconClass);
                    i.MergeAttribute("style", "margin-left:2px;");

                    a.InnerHtml += i.ToString();
                }

                a.InnerHtml += button.Text;
            }
            else
            {
                a.SetInnerText(button.Text);
            }

            return a.ToString() + form;
        }

        private string DoneDataInfo(ToolBoxItem item)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("btn btn-default disabled margin-left-10");
            span.SetInnerText($"{item.DataInfoText}: {item.DataInfoValue}");

            return span.ToString();
        }

        private string DoneRadioGroup(ToolBoxRadioGroup item)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("btn-group" + (!string.IsNullOrWhiteSpace(item.CssClass) ? " " + item.CssClass : ""));
            div.MergeAttribute("data-toggle", "buttons");

            if (item.RadioGroups != null && item.RadioGroups.Any())
            {
                var doneChecked = false;
                foreach (var radioButton in item.RadioGroups)
                {
                    var span = new TagBuilder("span");
                    span.SetInnerText(radioButton.Text);

                    var input = new TagBuilder("input");
                    input.MergeAttribute("type", "radio");
                    input.MergeAttribute("name", item.RadioGroupName);
                    input.MergeAttribute("value", radioButton.Value);
                    input.MergeAttribute("autocomplete", "off");

                    var label = new TagBuilder("label");
                    label.AddCssClass($"btn btn-{radioButton.Color.GetDisplayName()}" + (!doneChecked && radioButton.IsChecked ? " active" : ""));
                    label.InnerHtml += input + span.ToString();

                    div.InnerHtml += label.ToString();

                    if (radioButton.IsChecked)
                        doneChecked = true;

                }
            }

            return div.ToString();
        }
    }
}