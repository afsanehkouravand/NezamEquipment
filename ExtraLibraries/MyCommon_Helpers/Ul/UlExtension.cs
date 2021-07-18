using System;
using System.Linq;
using System.Web.Mvc;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Ul
{
    public class UlExtension
    {
        private readonly UlOption _option;

        public UlExtension(HtmlHelper htmlHelper, object model)
        {
            _option = new UlOption()
            {
                RequestContext = htmlHelper.ViewContext.RequestContext,
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

        public UlExtension Items(Action<UlDataItem> build)
        {
            build(new UlDataItem(_option));
            return this;
        }

        public UlExtension Options(Action<UlDataOption> build)
        {
            build(new UlDataOption(_option));
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

            var ul = new TagBuilder("ul") { InnerHtml = "" };
            ul.AddCssClass("nav nav-tabs well well-shadow");
            ul.MergeAttribute("role", "tablist");
            ul.MergeAttribute("style", "padding: 6px 6px 1px 0;");

            if (_option.Items.Any())
            {
                foreach (var item in _option.Items)
                {
                    if (_option.EnabledHaveAccessTo)
                    {
                        if (!string.IsNullOrWhiteSpace(item.HaveAccessToArea) ||
                            !string.IsNullOrWhiteSpace(item.AjaxHaveAccessToController) ||
                            !string.IsNullOrWhiteSpace(item.AjaxHaveAccessToAction))
                        {
                            var haveAccessTo = _option.HaveAccessToRoleAccess.Check(_option.HaveAccessToIsAdmin, item.HaveAccessToArea,
                                item.AjaxHaveAccessToController, item.AjaxHaveAccessToAction);
                            if (!haveAccessTo)
                            {
                                continue;
                            }
                        }
                    }

                    var li = new TagBuilder("li");
                    li.MergeAttribute("role", "presentation");
                    if (item.IsActive)
                    {
                        li.AddCssClass("active");
                    }

                    var a = new TagBuilder("a");
                    a.MergeAttribute("href", $"#{item.Id}");
                    a.MergeAttribute("aria-controls", item.Id);
                    a.MergeAttribute("role", "tab");
                    a.MergeAttribute("data-toggle", "tab");
                    if (!string.IsNullOrWhiteSpace(item.AjaxUrl))
                    {
                        a.MergeAttribute("data-tabajax", "true");
                        a.MergeAttribute("data-tabajax-url", item.AjaxUrl);
                    }
                    if (item.IsActive)
                    {
                        a.AddCssClass("activeloading");
                    }
                    a.SetInnerText(item.Title);

                    li.InnerHtml = a.ToString();

                    ul.InnerHtml += li.ToString();
                }
            }

            return new MvcHtmlString(ul.ToString());
        }
    }
}