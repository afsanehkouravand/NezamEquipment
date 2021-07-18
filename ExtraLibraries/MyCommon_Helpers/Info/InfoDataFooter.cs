using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Info
{
    public class InfoDataFooter
    {
        private readonly InfoOption _option;
        private readonly HtmlHelper _htmlHelper;

        public InfoDataFooter(InfoOption options, HtmlHelper htmlHelper)
        {
            _option = options;
            _htmlHelper = htmlHelper;
        }

        public InfoButton AddEditButton()
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_option.RequestContext);

            var button = new InfoButton
            {
                Text = "ویرایش",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "label label-primary" },
                    {"style", "margin:1px;" },
                },
                Href = url.Action("Edit", controllerName, new { area = areaName }),
                HaveAccessToAction = "Edit",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-edit",
                QureyStrings = new Dictionary<string, string>()
                {
                    {"id","0" }
                },
            };

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }
        public InfoButton AddEditButton(ActionResult t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public InfoButton AddEditButton(Task<ActionResult> t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public InfoButton AddEditButton(JsonResult t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public InfoButton AddEditButton(Task<JsonResult> t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        private InfoButton GetEditButton(string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new InfoButton
            {
                Text = text,
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                IconClass = "fa fa-edit",
                Href = info.Url,
            };

            if (!button.HtmlAttributes.ContainsKey("class"))
            {
                button.HtmlAttributes.Add("class", "label label-primary");
            }
            if (!button.HtmlAttributes.ContainsKey("style"))
            {
                button.HtmlAttributes.Add("style", "margin:1px;");
            }

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }

        public InfoButton AddDeleteButton(Guid id)
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_option.RequestContext);

            var button = new InfoButton
            {
                Text = "حذف",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "label label-danger" },
                    {"style", "margin:1px;" },
                    {"data-delete", "true" },
                    {"data-id", id.ToString() },
                    {"data-url", url.Action("Delete", controllerName, new { area = areaName }) },
                },
                HaveAccessToAction = "Delete",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-remove",
            };

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }
        public InfoButton AddDeleteButton(Guid id, ActionResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public InfoButton AddDeleteButton(Guid id, Task<ActionResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public InfoButton AddDeleteButton(Guid id, JsonResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public InfoButton AddDeleteButton(Guid id, Task<JsonResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        private InfoButton GetDeleteButton(Guid id, string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new InfoButton
            {
                Text = text,
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                IconClass = "fa fa-remove",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"data-delete", "true" },
                    {"data-id", id.ToString() },
                    {"data-url", info.Url },
                }
            };

            if (!button.HtmlAttributes.ContainsKey("class"))
            {
                button.HtmlAttributes.Add("class", "label label-danger");
            }
            if (!button.HtmlAttributes.ContainsKey("style"))
            {
                button.HtmlAttributes.Add("style", "margin:1px;");
            }

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }

        public InfoButton AddButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public InfoButton AddButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public InfoButton AddButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public InfoButton AddButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        private InfoButton GetButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new InfoButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
            };

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }

        public InfoButton AddAjaxButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public InfoButton AddAjaxButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public InfoButton AddAjaxButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public InfoButton AddAjaxButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        private InfoButton GetAjaxButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new InfoButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
            };

            button.HtmlAttributes.Add("data-ajax", "true");
            button.HtmlAttributes.Add("data-ajax-method", "post");
            button.HtmlAttributes.Add("data-ajax-loading", "#loading");
            button.HtmlAttributes.Add("data-ajax-complete", "OnComplete");
            button.HtmlAttributes.Add("data-ajax-begin", "OnBegin");
            button.HtmlAttributes.Add("data-ajax-failure", "OnFailure");
            button.HtmlAttributes.Add("data-ajax-success", "OnSuccess");
            button.HtmlAttributes.Add("data-ajax-url", info.Url);

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }


        public InfoButton AddButtonContent(string url)
        {
            var button = new InfoButton
            {
                HaveAccessToAction = string.Empty,
                HaveAccessToController = string.Empty,
                HaveAccessToArea = string.Empty,
                Href = url,
            };

            _option.EnablePanelFooter = true;
            _option.PanelFooterButtons.Add(button);

            return button;
        }

    }
}