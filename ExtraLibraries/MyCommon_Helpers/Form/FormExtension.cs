using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BotDetect;
using BotDetect.Web.Mvc;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Form
{
    public class FormExtension 
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly FormOption _option;

        public FormExtension(HtmlHelper htmlHelper, object model)
        {
            _htmlHelper = htmlHelper;
            _option = new FormOption()
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

        public FormExtension Options(Action<FormDataOption> buildOptions)
        {
            buildOptions(new FormDataOption(options: _option, htmlHelper: _htmlHelper));
            return this;
        }

        public FormExtension Items(Action<FormDataItem> buildItems)
        {
            buildItems(new FormDataItem(options: _option, htmlHelper: _htmlHelper));
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

            var list = new List<List<FormItem>>();
            var secondList = new List<FormItem>();
            var colume = 0;
            foreach (var item in _option.FormItems)
            {
                colume += item.ColSize;
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
                    colume = item.ColSize;
                }
            }
            if (secondList.Any())
            {
                list.Add(secondList);
            }

            var stringFormItems = string.Empty;

            foreach (var itemRow in list)
            {
                var row = new TagBuilder("div");
                row.AddCssClass("row margin-bottom-10");

                foreach (var item in itemRow)
                {
                    row.InnerHtml += _option.IsMaterialStyle ? DoneTagMaterial(item) : DoneTag(item);
                }

                stringFormItems += row.ToString();
            }

            if (_option.InsidePanelDiv)
            {
                var divPanelHeadingH5 = new TagBuilder("h5");
                divPanelHeadingH5.AddCssClass("margin-0");
                divPanelHeadingH5.SetInnerText(_option.InsidePanelDivTitle);

                var divPanelHeading = new TagBuilder("div");
                divPanelHeading.AddCssClass("panel-heading");
                divPanelHeading.InnerHtml += divPanelHeadingH5.ToString();

                var divPanelBody = new TagBuilder("div");
                divPanelBody.AddCssClass("panel-body");
                divPanelBody.InnerHtml += DoneForm(stringFormItems);

                var divPanel = new TagBuilder("div");
                divPanel.AddCssClass($"panel panel-{_option.InsidePanelDivColor.GetDisplayName()} defaultInfo" +
                                     (!string.IsNullOrWhiteSpace(_option.CssClass) ? " " + _option.CssClass : ""));
                divPanel.InnerHtml += divPanelHeading.ToString() + divPanelBody;

                html += divPanel.ToString();
            }
            else
            {
                html += DoneForm(stringFormItems);
            }

            return new MvcHtmlString(html);
        }

        private string DoneForm(string formItems)
        {
            var haveAccessTo = true;
            if (_option.EnabledHaveAccessTo)
            {
                haveAccessTo = _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                    _option.HaveAccessToArea, _option.HaveAccessToController, _option.HaveAccessToAction);
            }
            if (!haveAccessTo)
                return string.Empty;

            var rnd = new Random();
            var idRandom =  + rnd.Next(1, 999999);

            var inputSubmit = new TagBuilder("button");
            if (!string.IsNullOrWhiteSpace(_option.SubmitOnClick))
            {
                inputSubmit.MergeAttribute("onclick", _option.SubmitOnClick);
            }
            inputSubmit.MergeAttribute("type", _option.IsSubmit ? "submit" : "button");
            foreach (var item in _option.SubmitAttributes)
            {
                inputSubmit.MergeAttribute(item.Key, item.Value);
            }
            inputSubmit.AddCssClass(_option.SubmitAttributes.ContainsKey("class")
                ? $"btn btn-{_option.SubmitColor.GetDisplayName()} {_option.SubmitAttributes["class"]}"
                : $"btn btn-{_option.SubmitColor.GetDisplayName()}");

            inputSubmit.SetInnerText(_option.SubmitText);

            var loadingTag = string.Empty;
            if (_option.FormMethod == EFormMethodType.Post)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("btn btn-default disabled margin-right-20 display-none");
                span.MergeAttribute("id", _option.Loading + idRandom);

                var i = new TagBuilder("i");
                i.AddCssClass("fa fa-refresh fa-spin");
                i.MergeAttribute("style", "margin-left:2px; margin-right:2px");

                var spanText = new TagBuilder("span");
                spanText.SetInnerText("لطفا صبر کنید");

                span.InnerHtml += i.ToString();
                span.InnerHtml += spanText.ToString();

                loadingTag = span.ToString();
            }

            var dataInfos = string.Empty;
            foreach (var dataInfo in _option.DataInfos)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("btn btn-default disabled");
                span.MergeAttribute("style", "margin:2px 10px 0 0;");
                span.SetInnerText($"{dataInfo.Key}: {dataInfo.Value}");

                dataInfos += span.ToString();
            }

            var buttons = string.Empty;
            foreach (var item in _option.FormButtons)
            {
                buttons += DoneButton(item);
            }

            var fieldsetDivRowDivCol12 = new TagBuilder("div");
            fieldsetDivRowDivCol12.AddCssClass("col-sm-12 margin-top-20");
            fieldsetDivRowDivCol12.InnerHtml += inputSubmit + buttons + loadingTag + dataInfos;

            var fieldsetDivRow = new TagBuilder("div");
            fieldsetDivRow.AddCssClass("row");
            fieldsetDivRow.InnerHtml += fieldsetDivRowDivCol12.ToString();

            var fieldset = new TagBuilder("fieldset");
            if (_option.IsDisabled)
            {
                fieldset.MergeAttribute("disabled", "disabled");
            }
            fieldset.AddCssClass("fieldset");
            if (_option.FormMethod == EFormMethodType.Post)
            {
                fieldset.InnerHtml += _htmlHelper.AntiForgeryToken();
                fieldset.InnerHtml += _htmlHelper.ValidationSummary(true, null, new { @class = "text-danger" });
            }
            fieldset.InnerHtml += formItems;
            foreach (var hiddenProperty in _option.HiddenProperties)
            {
                fieldset.InnerHtml += _htmlHelper.Hidden(hiddenProperty.Key, hiddenProperty.Value);
            }
            fieldset.InnerHtml += fieldsetDivRow.ToString();

            var div = new TagBuilder("div");
            if (_option.HideFormWell == false)
            {
                div.AddCssClass("well well-white well-shadow");
            }
            div.InnerHtml = fieldset.ToString();

            var form = new TagBuilder("form");
            form.MergeAttribute("method", _option.FormMethod.GetDisplayName());
            form.MergeAttribute("id", $"form{idRandom}");
            form.MergeAttribute("action", _option.Url);
            if (_option.IsAjaxForm)
            {
                form.MergeAttribute("data-ajax", "true");
                form.MergeAttribute("data-ajax-begin", _option.OnBegin);
                form.MergeAttribute("data-ajax-complete", _option.OnComplete);
                form.MergeAttribute("data-ajax-failure", _option.OnFailure);
                form.MergeAttribute("data-ajax-success", _option.OnSuccess);
                if (!string.IsNullOrWhiteSpace(_option.UpdateTarget))
                {
                    form.MergeAttribute("data-ajax-update", "#" + _option.UpdateTarget);
                }
                form.MergeAttribute("data-ajax-loading", "#" + _option.Loading + idRandom);
                form.MergeAttribute("data-ajax-method", _option.FormMethod.GetDisplayName());
                form.MergeAttribute("data-ajax-url", _option.Url);
            }
            form.InnerHtml += div.ToString();
            if (!string.IsNullOrWhiteSpace(_option.GoToPage))
            {
                form.MergeAttribute("data-gotopage", _option.GoToPage);
                form.MergeAttribute("onsubmit", "formonsubmit(this)");
            }

            return form.ToString();
        }

        private string DoneTag(FormItem formItem)
        {
            var tag = new TagBuilder(formItem.Tag);

            if (_option.IsComplex && formItem.HtmlAttributes.ContainsKey("data-targetselector"))
            {
                var namedata = _option.IsComplex ? $"{_option.Prefix}.{formItem.Name}" : formItem.Name;
                formItem.HtmlAttributes["data-targetselector"] = "#" + namedata.GetIdFromName();
            }

            foreach (var htmlAttribute in formItem.HtmlAttributes)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (formItem.Tag == "select")
            {
                if (formItem.UseChooseOption)
                {
                    var chooseSelect = new TagBuilder("option");
                    chooseSelect.SetInnerText("انتخاب کنید");
                    chooseSelect.MergeAttribute("value", "");
                    tag.InnerHtml += chooseSelect.ToString();
                }

                if (formItem.Child.Any())
                {
                    foreach (var item in formItem.Child)
                    {
                        var oSelect = new TagBuilder("option");
                        oSelect.MergeAttribute("value", item.Key);
                        oSelect.SetInnerText(item.Value);
                        if (_option.UseModelValue && !string.IsNullOrWhiteSpace(formItem.Value) && item.Key == formItem.Value)
                        {
                            oSelect.MergeAttribute("selected", "selected");
                        }

                        tag.InnerHtml += oSelect.ToString();
                    }
                }
                if (formItem.ChildGroup.Any())
                {
                    foreach (var groupitem in formItem.ChildGroup)
                    {
                        var ogroup = new TagBuilder("optgroup");
                        ogroup.MergeAttribute("label", groupitem.Key);

                        foreach (var item in groupitem.Value)
                        {
                            var oSelect = new TagBuilder("option");
                            oSelect.MergeAttribute("value", item.Key);
                            oSelect.SetInnerText(item.Value);
                            if (_option.UseModelValue && !string.IsNullOrWhiteSpace(formItem.Value) && item.Key == formItem.Value)
                            {
                                oSelect.MergeAttribute("selected", "selected");
                            }

                            ogroup.InnerHtml += oSelect.ToString();
                        }

                        tag.InnerHtml += ogroup.ToString();
                    }
                }
            } 
            else if (formItem.Tag == "legend")
            {
                tag.SetInnerText(formItem.Text);
            }

            if (_option.UseModelValue && !string.IsNullOrWhiteSpace(formItem.Value) && formItem.Tag != "select")
            {
                if (formItem.Tag == "textarea")
                {
                    tag.SetInnerText(formItem.Value);
                }
                else
                {
                    tag.MergeAttribute("value", formItem.Value);
                }
            }

            var name = _option.IsComplex ? $"{_option.Prefix}.{formItem.Name}" : formItem.Name;
            if (!string.IsNullOrWhiteSpace(name))
            {
                tag.MergeAttribute("name", name);
                tag.MergeAttribute("id", name.GetIdFromName());
            }
            if (formItem.DataList != null && formItem.DataList.Any())
            {
                tag.MergeAttribute("list", name.GetIdFromName() + "_datalist");
            }

            if (!string.IsNullOrWhiteSpace(formItem.Class))
            {
                tag.MergeAttribute("class", formItem.Class);
            }

            if (!string.IsNullOrWhiteSpace(formItem.Style))
            {
                tag.MergeAttribute("style", formItem.Style);
            }
            if (!string.IsNullOrWhiteSpace(formItem.Type))
            {
                tag.MergeAttribute("type", formItem.Type);
            }

            var spanTag = string.Empty;
            if (formItem.DataVal)
            {
                tag.MergeAttribute("data-val", "true");
                tag.MergeAttribute("data-val-required", formItem.DataValRequired);

                var span = new TagBuilder("span");
                span.MergeAttribute("class", "field-validation-valid text-danger");
                span.MergeAttribute("data-valmsg-for", name);
                span.MergeAttribute("data-valmsg-replace", "true");

                spanTag = span.ToString();
            }

            var labelTag = string.Empty;
            if (!string.IsNullOrWhiteSpace(formItem.Label))
            {
                var label = new TagBuilder("label");
                label.SetInnerText(formItem.Label);
                label.MergeAttribute("for", name.GetIdFromName());

                labelTag = label.ToString();
            }

            var div  = new TagBuilder("div");
            div.AddCssClass($"col-sm-{formItem.ColSize}");

            var datalist = string.Empty;
            if (formItem.DataList != null && formItem.DataList.Any())
            {
                var datalistTag = new TagBuilder("datalist");
                datalistTag.MergeAttribute("id", name.GetIdFromName() + "_datalist");
                foreach (var item in formItem.DataList)
                {
                    var o = new TagBuilder("option");
                    o.MergeAttribute("value", item);

                    datalistTag.InnerHtml += o.ToString();
                }

                datalist = datalistTag.ToString();
            }

            var captcha = new TagBuilder("div");

            if (formItem.IsCaptcha)
            {
                var exampleCaptcha = new MvcCaptcha(formItem.CaptchaName)
                {
                    SoundEnabled = false,
                    CodeStyle = CodeStyle.Numeric,
                    UserInputID = formItem.Name,
                    ReloadEnabled = true,
                    AutoClearInput = true,
                    ImageSize = new Size(250, 50)
                };
                captcha.InnerHtml += _htmlHelper.Captcha(exampleCaptcha);
            }

            div.InnerHtml = labelTag + captcha + tag + datalist + spanTag;
            return div.ToString();
        }

        private string DoneTagMaterial(FormItem formItem)
        {
            var tag = new TagBuilder(formItem.Tag);

            if (_option.IsComplex && formItem.HtmlAttributes.ContainsKey("data-targetselector"))
            {
                var namedata = _option.IsComplex ? $"{_option.Prefix}.{formItem.Name}" : formItem.Name;
                formItem.HtmlAttributes["data-targetselector"] = "#" + namedata.GetIdFromName();
            }

            foreach (var htmlAttribute in formItem.HtmlAttributes)
            {
                tag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value);
            }

            if (formItem.Tag == "select")
            {
                var chooseSelect = new TagBuilder("option");
                chooseSelect.SetInnerText("");
                chooseSelect.MergeAttribute("value", "");
                tag.InnerHtml += chooseSelect.ToString();

                if (formItem.Child.Any())
                {
                    foreach (var item in formItem.Child)
                    {
                        var oSelect = new TagBuilder("option");
                        oSelect.MergeAttribute("value", item.Key);
                        oSelect.SetInnerText(item.Value);
                        if (_option.UseModelValue && !string.IsNullOrWhiteSpace(formItem.Value) && item.Key == formItem.Value)
                        {
                            oSelect.MergeAttribute("selected", "selected");
                        }

                        tag.InnerHtml += oSelect.ToString();
                    }
                }
            }
            else if (formItem.Tag == "legend")
            {
                tag.SetInnerText(formItem.Text);
            }

            if (_option.UseModelValue && !string.IsNullOrWhiteSpace(formItem.Value) && formItem.Tag != "select")
            {
                if (formItem.Tag == "textarea")
                {
                    tag.SetInnerText(formItem.Value);
                }
                else
                {
                    tag.MergeAttribute("value", formItem.Value);
                }
            }

            var name = _option.IsComplex ? $"{_option.Prefix}.{formItem.Name}" : formItem.Name;
            if (!string.IsNullOrWhiteSpace(name))
            {
                tag.MergeAttribute("name", name);
                tag.MergeAttribute("id", name.GetIdFromName());
            }

            if (!string.IsNullOrWhiteSpace(formItem.Class))
            {
                tag.MergeAttribute("class", formItem.Class);
            }

            if (!string.IsNullOrWhiteSpace(formItem.Style))
            {
                tag.MergeAttribute("style", formItem.Style);
            }
            if (!string.IsNullOrWhiteSpace(formItem.Type))
            {
                tag.MergeAttribute("type", formItem.Type);
            }

            var spanTag = string.Empty;
            if (formItem.DataVal)
            {
                tag.MergeAttribute("data-val", "true");
                tag.MergeAttribute("data-val-required", formItem.DataValRequired);

                var span = new TagBuilder("span");
                span.MergeAttribute("class", "field-validation-valid text-danger");
                span.MergeAttribute("data-valmsg-for", name);
                span.MergeAttribute("data-valmsg-replace", "true");

                spanTag = span.ToString();
            }

            var labelTag = string.Empty;
            if (!string.IsNullOrWhiteSpace(formItem.Label))
            {
                var label = new TagBuilder("label");
                label.AddCssClass("control-label");
                label.SetInnerText(formItem.Label);
                label.MergeAttribute("for", name.GetIdFromName());

                labelTag = label.ToString();
            }

            var divForm = new TagBuilder("div");
            divForm.AddCssClass("form-group label-floating " + (string.IsNullOrWhiteSpace(formItem.Value) ? "is-empty" : ""));
            divForm.InnerHtml = labelTag + tag + spanTag;

            var div = new TagBuilder("div");
            div.AddCssClass($"col-sm-{formItem.ColSize}");
            div.InnerHtml = divForm.ToString();

            return div.ToString();
        }

        private string DoneButton(FormButton button)
        {
            var haveAccessToButton = true;
            if (_option.EnabledHaveAccessTo)
                haveAccessToButton =
                    _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin,
                        button.HaveAccessToArea, button.HaveAccessToController, button.HaveAccessToAction);

            if (!haveAccessToButton)
                return string.Empty;

            var tag = new TagBuilder("a");

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

            if (!string.IsNullOrWhiteSpace(button.IconClass))
            {
                var i = new TagBuilder("i");
                i.AddCssClass(button.IconClass);
                i.MergeAttribute("style", "margin-left:2px;");

                tag.InnerHtml += i.ToString();

                tag.InnerHtml += button.Text;
            }
            else
            {
                tag.SetInnerText(button.Text);
            }

            return tag.ToString();
        }

    }
}