using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Framework.Helper
{
    public static class DefaultToolButtonsForHelper
    {
        public static DefaultToolButtonsForExtension DefaultToolButtonsFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            HaveAccessToViewModel haveAccessTo)
        {
            return new DefaultToolButtonsForExtension(htmlHelper: htmlHelper, haveAccessTo: haveAccessTo);
        }
    }

    public class DefaultToolButtonsForExtension
    {
        private readonly HtmlHelper _htmlHelper;
        protected DefaultToolButtonsForOptions Options;

        public DefaultToolButtonsForExtension(HtmlHelper htmlHelper, HaveAccessToViewModel haveAccessTo)
        {
            _htmlHelper = htmlHelper;
            Options = new DefaultToolButtonsForOptions
            {
                HaveAccessTo = haveAccessTo,
            };
        }

        public DefaultToolButtonsForExtension Items(Action<DefaultToolButtonsForData> buildOptions)
        {
            buildOptions(new DefaultToolButtonsForData(options: Options));
            return this;
        }

        public MvcHtmlString Done()
        {
            var currentControllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var div = new TagBuilder("div") { InnerHtml = "" };
            div.AddCssClass("well well-shadow");

            if (Options.DefaultToolButtonsForItems.Any())
            {
                foreach (var item in Options.DefaultToolButtonsForItems)
                {
                    if (!string.IsNullOrWhiteSpace(item.HaveAccessToAction))
                    {
                        if (string.IsNullOrWhiteSpace(item.HaveAccessToController))
                            item.HaveAccessToController = currentControllerName;
                        
                        if (!Options.HaveAccessTo.Check(item.HaveAccessToController, item.HaveAccessToAction))
                        {
                            continue;
                        }
                    }

                    var a = new TagBuilder("a");
                    a.SetInnerText(item.Title);

                    if (!string.IsNullOrWhiteSpace(item.Class))
                    {
                        a.MergeAttribute("class", item.Class);
                    }

                    if (!string.IsNullOrWhiteSpace(item.Href))
                    {
                        a.MergeAttribute("href", item.Href);
                    }

                    if (item.HttpAttributes != null && item.HttpAttributes.Any())
                    {
                        foreach (var httpAttribute in item.HttpAttributes)
                        {
                            a.MergeAttribute(httpAttribute.Key, httpAttribute.Value);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(item.FontAwesome))
                    {
                        var i = new TagBuilder("i");
                        i.AddCssClass(item.FontAwesome);

                        a.InnerHtml = $"{i} {item.Title}";
                    }

                    div.InnerHtml += a.ToString();
                }
            }

            return new MvcHtmlString(div.ToString());
        }

    }

    public class DefaultToolButtonsForData
    {
        protected DefaultToolButtonsForOptions Options;

        public DefaultToolButtonsForData(DefaultToolButtonsForOptions options)
        {
            Options = options;
        }

        public DefaultToolButtonsForData AddFor(string title,string @class = null, string href = null,
            string haveAccessToController = null, string haveAccessToAction = null,
            IDictionary<string, string> httpAttributes = null, string fontAwsome = null)
        {
            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
            {
                Title = title,
                Class = @class,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToController = haveAccessToController,
                Href = href,
                HttpAttributes = httpAttributes,
                FontAwesome = fontAwsome,
            });
            return this;
        }

        public DefaultToolButtonsForData AddDeleteFor(long id, string url, string title = "حذف", 
            string haveAccessToAction = "Delete", string haveAccessToController = null)
        {
            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
            {
                Title = title,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToController = haveAccessToController,
                HttpAttributes = new Dictionary<string, string>()
                {
                    {"data-delete","true" },
                    {"data-url", url },
                    {"data-id", id.ToString() },
                    {"class", "btn btn-danger  margin-left-10" },
                },
                FontAwesome = "fa fa-remove",
            });
            return this;
        }

        public DefaultToolButtonsForData AddEditFor(string url, string title = "ویرایش",
            string haveAccessToAction = "Edit", string haveAccessToController = null)
        {
            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
            {
                Title = title,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToController = haveAccessToController,
                HttpAttributes = new Dictionary<string, string>()
                {
                    {"class", "btn btn-primary  margin-left-10" },
                    {"href", url },
                },
                FontAwesome = "fa fa-edit",
            });
            return this;
        }

        public DefaultToolButtonsForData AddPopUpFor(string title, string url, string popupTitle,
            string haveAccessToAction, string haveAccessToController = null)
        {
            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
            {
                Title = title,
                HaveAccessToAction = haveAccessToAction,
                HaveAccessToController = haveAccessToController,
                HttpAttributes = new Dictionary<string, string>()
                {
                    {"data-popup", "true"},
                    {"data-url", url},
                    {"data-title", popupTitle},
                    {"class", "btn btn-info margin-left-10"}
                },
            });
            return this;
        }

    }

    public class DefaultToolButtonsForOptions
    {
        public DefaultToolButtonsForOptions()
        {
            DefaultToolButtonsForItems = new List<DefaultToolButtonsForItem>();
        }

        public List<DefaultToolButtonsForItem> DefaultToolButtonsForItems { get; set; }

        public HaveAccessToViewModel HaveAccessTo { get; set; }
    }

    public class DefaultToolButtonsForItem
    {
        public string Title { get; set; }
        public string Class { get; set; }
        public string Href { get; set; }
        public string FontAwesome { get; set; }
        public IDictionary<string, string> HttpAttributes { get; set; }
        public string HaveAccessToController { get; set; }
        public string HaveAccessToAction { get; set; }
    }

}