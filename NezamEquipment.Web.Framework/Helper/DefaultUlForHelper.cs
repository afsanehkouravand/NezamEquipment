using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class DefaultUlForHelper
    {
        public static DefaultUlForExtension DefaultUlFor<TModel>(this HtmlHelper<TModel> htmlHelper, HaveAccessToViewModel haveAccessTo)
        {
            return new DefaultUlForExtension(haveAccessTo: haveAccessTo);
        }
    }

    public class DefaultUlForExtension
    {
        protected DefaultUlForOptions Options;

        public DefaultUlForExtension(HaveAccessToViewModel haveAccessTo)
        {
            Options = new DefaultUlForOptions
            {
                HaveAccessTo = haveAccessTo,
            };
        }

        public DefaultUlForExtension Items(Action<DefaultUlForData> buildOptions)
        {
            buildOptions(new DefaultUlForData(options: Options));
            return this;
        }

        public MvcHtmlString Done()
        {
            var ul = new TagBuilder("ul") { InnerHtml = "" };
            ul.AddCssClass("nav nav-tabs well well-shadow");
            ul.MergeAttribute("role", "tablist");
            ul.MergeAttribute("style", "padding: 6px 6px 1px 0;");

            if (Options.DefaultUlForItems.Any())
            {
                foreach (var defaultUlForItem in Options.DefaultUlForItems)
                {
                    var haveAccessTo = Options.HaveAccessTo.Check(defaultUlForItem.AjaxHaveAccessToController, defaultUlForItem.AjaxHaveAccessToAction);
                    if (!haveAccessTo)
                    {
                        continue;
                    }

                    var li = new TagBuilder("li");
                    li.MergeAttribute("role", "presentation");
                    if (defaultUlForItem.IsActive)
                    {
                        li.AddCssClass("active");
                    }

                    var a = new TagBuilder("a");
                    a.MergeAttribute("href",$"#{defaultUlForItem.Id}");
                    a.MergeAttribute("aria-controls", defaultUlForItem.Id);
                    a.MergeAttribute("role", "tab");
                    a.MergeAttribute("data-toggle", "tab");
                    if (!string.IsNullOrWhiteSpace(defaultUlForItem.AjaxUrl))
                    {
                        a.MergeAttribute("data-tabajax", "true");
                        a.MergeAttribute("data-tabajax-url", defaultUlForItem.AjaxUrl);
                    }
                    if (defaultUlForItem.IsActive)
                    {
                        a.AddCssClass("activeloading");
                    }
                    a.SetInnerText(defaultUlForItem.Title);

                    li.InnerHtml = a.ToString();

                    ul.InnerHtml += li.ToString();
                }
            }

            return new MvcHtmlString(ul.ToString());
        }

    }

    public class DefaultUlForData
    {
        protected DefaultUlForOptions Options;

        public DefaultUlForData(DefaultUlForOptions options)
        {
            Options = options;
        }

        public DefaultUlForData AddFor(string id, string title, bool isActive = false, string ajaxUrl = null, string haveAccessToController = null,
            string haveAccessToAction = null)
        {
            Options.DefaultUlForItems.Add(new DefaultUlForItem()
            {
                Id = id,
                Title = title,
                IsActive = isActive,
                AjaxUrl = ajaxUrl,
                AjaxHaveAccessToController = haveAccessToController,
                AjaxHaveAccessToAction = haveAccessToAction,
            });
            return this;
        }

    }

    public class DefaultUlForOptions
    {
        public DefaultUlForOptions()
        {
            DefaultUlForItems = new List<DefaultUlForItem>();
        }

        public List<DefaultUlForItem> DefaultUlForItems { get; set; }

        public HaveAccessToViewModel HaveAccessTo { get; set; }
    }

    public class DefaultUlForItem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsAjax { get; set; }
        public string AjaxUrl { get; set; }
        public string AjaxHaveAccessToController { get; set; }
        public string AjaxHaveAccessToAction { get; set; }

        public bool IsActive { get; set; }
    }

}