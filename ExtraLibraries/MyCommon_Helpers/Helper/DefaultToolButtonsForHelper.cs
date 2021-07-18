//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using MyCommon.Helpers.Security;

//namespace MyCommon.Helpers.Helper
//{
//    public static class DefaultToolButtonsForHelper
//    {
//        public static DefaultToolButtonsForExtension DefaultToolButtonsFor<TModel>(this HtmlHelper<TModel> htmlHelper,
//            object haveAccessTo, bool addLoading = false, string addLoadingId = "loading")
//        {
//            return new DefaultToolButtonsForExtension(htmlHelper: htmlHelper, haveAccessTo: haveAccessTo, addLoading: addLoading, addLoadingId: addLoadingId);
//        }
//    }

//    public class DefaultToolButtonsForExtension
//    {
//        private readonly HtmlHelper _htmlHelper;
//        protected DefaultToolButtonsForOptions Options;

//        public DefaultToolButtonsForExtension(HtmlHelper htmlHelper, object haveAccessTo, bool addLoading, string addLoadingId)
//        {
//            _htmlHelper = htmlHelper;
//            Options = new DefaultToolButtonsForOptions
//            {
//                HaveAccessTo = haveAccessTo,
//                AddLoading = addLoading,
//                AddLoadingId = addLoadingId,
//            };
//        }

//        public DefaultToolButtonsForExtension Items(Action<DefaultToolButtonsForData> buildOptions)
//        {
//            buildOptions(new DefaultToolButtonsForData(options: Options));
//            return this;
//        }

//        public MvcHtmlString Done()
//        {
//            var currentControllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

//            var div = new TagBuilder("div") { InnerHtml = "" };
//            div.AddCssClass("well well-shadow");

//            if (Options.DefaultToolButtonsForItems.Any())
//            {
//                foreach (var item in Options.DefaultToolButtonsForItems)
//                {
//                    if (!string.IsNullOrWhiteSpace(item.HaveAccessToAction))
//                    {
//                        if (string.IsNullOrWhiteSpace(item.HaveAccessToController))
//                            item.HaveAccessToController = currentControllerName;

//                        if (!Options.HaveAccessTo.Check("Admin", item.HaveAccessToController, item.HaveAccessToAction))
//                        {
//                            continue;
//                        }
//                    }

//                    var a = new TagBuilder("a");
//                    a.SetInnerText(item.Title);

//                    if (!string.IsNullOrWhiteSpace(item.Class))
//                    {
//                        a.MergeAttribute("class", item.Class);
//                    }

//                    if (!string.IsNullOrWhiteSpace(item.Href))
//                    {
//                        a.MergeAttribute("href", item.Href);
//                    }

//                    if (item.HttpAttributes != null && item.HttpAttributes.Any())
//                    {
//                        foreach (var httpAttribute in item.HttpAttributes)
//                        {
//                            a.MergeAttribute(httpAttribute.Key, httpAttribute.Value);
//                        }
//                    }

//                    if (!string.IsNullOrWhiteSpace(item.FontAwesome))
//                    {
//                        var i = new TagBuilder("i");
//                        i.AddCssClass(item.FontAwesome);

//                        a.InnerHtml = $"{i} {item.Title}";
//                    }

//                    div.InnerHtml += a.ToString();
//                }
//            }

//            if (Options.AddLoading)
//            {
//                var divTag = new TagBuilder("span");
//                divTag.AddCssClass("btn btn-default disabled margin-right-20 display-none");
//                divTag.GenerateId(Options.AddLoadingId);

//                var iTag = new TagBuilder("i");
//                iTag.AddCssClass("fa fa-refresh fa-spin");

//                var spanTag = new TagBuilder("span");
//                spanTag.SetInnerText("لطفا صبر کنید");

//                divTag.InnerHtml += iTag;
//                divTag.InnerHtml += spanTag;

//                div.InnerHtml += divTag.ToString();
//            }

//            return new MvcHtmlString(div.ToString());
//        }

//    }

//    public class DefaultToolButtonsForData
//    {
//        protected DefaultToolButtonsForOptions Options;

//        public DefaultToolButtonsForData(DefaultToolButtonsForOptions options)
//        {
//            Options = options;
//        }

//        public DefaultToolButtonsForData AddFor(string title, string @class = null, string href = null,
//            string haveAccessToController = null, string haveAccessToAction = null,
//            IDictionary<string, string> httpAttributes = null, string fontAwsome = null)
//        {
//            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
//            {
//                Title = title,
//                Class = @class,
//                HaveAccessToAction = haveAccessToAction,
//                HaveAccessToController = haveAccessToController,
//                Href = href,
//                HttpAttributes = httpAttributes,
//                FontAwesome = fontAwsome,
//            });
//            return this;
//        }

//        public DefaultToolButtonsForData AddDeleteFor(long id, string url, string title = "حذف",
//            string haveAccessToAction = "Delete", string haveAccessToController = null, string gotopage = null)
//        {
//            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
//            {
//                Title = title,
//                HaveAccessToAction = haveAccessToAction,
//                HaveAccessToController = haveAccessToController,
//                HttpAttributes = new Dictionary<string, string>()
//                {
//                    {"data-delete", "true"},
//                    {"data-url", url},
//                    {"data-id", id.ToString()},
//                    {"class", "btn btn-danger  margin-left-10"},
//                    {"data-gotopage", gotopage}
//                },
//                FontAwesome = "fa fa-remove",
//            });
//            return this;
//        }

//        public DefaultToolButtonsForData AddEditFor(string url, string title = "ویرایش",
//            string haveAccessToAction = "Edit", string haveAccessToController = null)
//        {
//            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
//            {
//                Title = title,
//                HaveAccessToAction = haveAccessToAction,
//                HaveAccessToController = haveAccessToController,
//                HttpAttributes = new Dictionary<string, string>()
//                {
//                    {"class", "btn btn-primary  margin-left-10" },
//                    {"href", url },
//                },
//                FontAwesome = "fa fa-edit",
//            });
//            return this;
//        }

//        public DefaultToolButtonsForData AddPopUpFor(string title, string url, string popupTitle,
//            string haveAccessToAction, string haveAccessToController = null, string @class = null)
//        {
//            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
//            {
//                Title = title,
//                HaveAccessToAction = haveAccessToAction,
//                HaveAccessToController = haveAccessToController,
//                HttpAttributes = new Dictionary<string, string>()
//                {
//                    {"data-popup", "true"},
//                    {"data-url", url},
//                    {"data-title", popupTitle},
//                    {"class", !string.IsNullOrWhiteSpace(@class) ? @class : "btn btn-default margin-left-10"},
//                },
//            });
//            return this;
//        }

//        public DefaultToolButtonsForData AddAjaxButtonFor(string title, string url, string @class = null,
//            string ajaxBegin = "OnBegin", string ajaxComplete = "OnComplete", string ajaxFailure = "OnFailure",
//            string ajaxSuccess = "OnSuccess", string ajaxLoading = "#loading", string haveAccessToAction = null,
//            string haveAccessToController = null, string gotopage = null)
//        {
//            Options.DefaultToolButtonsForItems.Add(new DefaultToolButtonsForItem()
//            {
//                Title = title,
//                HaveAccessToAction = haveAccessToAction,
//                HaveAccessToController = haveAccessToController,
//                HttpAttributes = new Dictionary<string, string>()
//                {
//                    {"data-ajax", "true"},
//                    {"data-ajax-method", "post"},
//                    {"data-ajax-loading", ajaxLoading},
//                    {"data-ajax-begin", ajaxBegin},
//                    {"data-ajax-complete", ajaxComplete},
//                    {"data-ajax-failure", ajaxFailure},
//                    {"data-ajax-success", ajaxSuccess},
//                    {"data-ajax-url", url},
//                    {"href", url},
//                    {"class", !string.IsNullOrWhiteSpace(@class) ? @class : "btn btn-default margin-left-10"},
//                    {"data-gotopage", gotopage},
//                },
//            });
//            return this;
//        }

//    }

//    public class DefaultToolButtonsForOptions
//    {
//        public DefaultToolButtonsForOptions()
//        {
//            DefaultToolButtonsForItems = new List<DefaultToolButtonsForItem>();
//        }

//        public List<DefaultToolButtonsForItem> DefaultToolButtonsForItems { get; set; }

//        public object HaveAccessTo { get; set; }

//        public bool AddLoading { get; set; }
//        public string AddLoadingId { get; set; }
//    }

//    public class DefaultToolButtonsForItem
//    {
//        public string Title { get; set; }
//        public string Class { get; set; }
//        public string Href { get; set; }
//        public string FontAwesome { get; set; }
//        public IDictionary<string, string> HttpAttributes { get; set; }
//        public string HaveAccessToController { get; set; }
//        public string HaveAccessToAction { get; set; }
//    }

//}