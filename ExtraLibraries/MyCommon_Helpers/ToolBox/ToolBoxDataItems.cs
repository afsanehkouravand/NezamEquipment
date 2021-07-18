using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.ToolBox
{
    public class ToolBoxDataItems
    {
        private readonly ToolBoxOption _option;
        private readonly HtmlHelper _htmlHelper;

        public ToolBoxDataItems(ToolBoxOption option, HtmlHelper htmlHelper)
        {
            _option = option;
            _htmlHelper = htmlHelper;
        }

        public void AddDetailsButton()
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new ToolBoxButton()
            {
                Text = "اطلاعات",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "btn btn-info margin-left-10" },
                },
                Href = url.Action("Details", controllerName, new { area = areaName }),
                HaveAccessToAction = "Details",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-info",
                QureyStrings = new Dictionary<string, string>()
                {
                    {"id","Id" }
                },
            };

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);
        }
        public ToolBoxButton AddDetailsButton(ActionResult t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        public ToolBoxButton AddDetailsButton(Task<ActionResult> t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        public ToolBoxButton AddDetailsButton(JsonResult t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        public ToolBoxButton AddDetailsButton(Task<JsonResult> t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        private ToolBoxButton GetDetailsButton(string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
            {
                Text = text,
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                IconClass = "fa fa-info",
                Href = info.Url,
            };

            if (!button.HtmlAttributes.ContainsKey("class"))
            {
                button.HtmlAttributes.Add("class", "btn btn-info margin-left-10");
            }

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public void AddEditButton()
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new ToolBoxButton
            {
                Text = "ویرایش",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "btn btn-primary margin-left-10" },
                },
                Href = url.Action("Edit", controllerName, new { area = areaName }),
                HaveAccessToAction = "Edit",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-edit",
                QureyStrings = new Dictionary<string, string>()
                {
                    {"id","Id" }
                },
            };

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);
        }
        public ToolBoxButton AddEditButton(ActionResult t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public ToolBoxButton AddEditButton(Task<ActionResult> t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public ToolBoxButton AddEditButton(JsonResult t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public ToolBoxButton AddEditButton(Task<JsonResult> t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        private ToolBoxButton GetEditButton(string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
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
                button.HtmlAttributes.Add("class", "btn btn-primary margin-left-10");
            }

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public void AddDeleteButton(Guid id)
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new ToolBoxButton
            {
                Text = "حذف",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "btn btn-danger margin-left-10" },
                    {"data-delete", "true" },
                    {"data-id", id.ToString() },
                    {"data-url", url.Action("Delete", controllerName, new { area = areaName }) },
                },
                HaveAccessToAction = "Delete",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-remove",
            };

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);
        }
        public ToolBoxButton AddDeleteButton(Guid id, ActionResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public ToolBoxButton AddDeleteButton(Guid id, Task<ActionResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public ToolBoxButton AddDeleteButton(Guid id, JsonResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public ToolBoxButton AddDeleteButton(Guid id, Task<JsonResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        private ToolBoxButton GetDeleteButton(Guid id, string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
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
                button.HtmlAttributes.Add("class", "btn btn-danger margin-left-10");
            }

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public void AddDeleteButton(long id)
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new ToolBoxButton
            {
                Text = "حذف",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "btn btn-danger margin-left-10" },
                    {"data-delete", "true" },
                    {"data-id", id.ToString() },
                    {"data-url", url.Action("Delete", controllerName, new { area = areaName }) },
                },
                HaveAccessToAction = "Delete",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-remove",
            };

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);
        }
        public ToolBoxButton AddDeleteButton(long id, ActionResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public ToolBoxButton AddDeleteButton(long id, Task<ActionResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public ToolBoxButton AddDeleteButton(long id, JsonResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        public ToolBoxButton AddDeleteButton(long id, Task<JsonResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(id, text, info);
        }
        private ToolBoxButton GetDeleteButton(long id, string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
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
                button.HtmlAttributes.Add("class", "btn btn-danger margin-left-10");
            }

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public ToolBoxButton AddButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public ToolBoxButton AddButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public ToolBoxButton AddButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public ToolBoxButton AddButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        private ToolBoxButton GetButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
            };

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public ToolBoxButton AddAjaxButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public ToolBoxButton AddAjaxButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public ToolBoxButton AddAjaxButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public ToolBoxButton AddAjaxButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        private ToolBoxButton GetAjaxButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
                IsAjaxForm = true,
            };

            button.HtmlAttributes.Add("data-ajax", "true");
            button.HtmlAttributes.Add("data-ajax-method", "post");
            button.HtmlAttributes.Add("data-ajax-loading", "#loading");
            button.HtmlAttributes.Add("data-ajax-complete", "OnComplete");
            button.HtmlAttributes.Add("data-ajax-begin", "OnBegin");
            button.HtmlAttributes.Add("data-ajax-failure", "OnFailure");
            button.HtmlAttributes.Add("data-ajax-success", "OnSuccess");
            button.HtmlAttributes.Add("data-ajax-url", info.Url);

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public ToolBoxButton AddPopupButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        public ToolBoxButton AddPopupButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        public ToolBoxButton AddPopupButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        public ToolBoxButton AddPopupButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        private ToolBoxButton GetPopupButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new ToolBoxButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
            };

            button.HtmlAttributes.Add("data-popup", "true");
            button.HtmlAttributes.Add("data-url", info.Url);
            button.HtmlAttributes.Add("data-title", button.Text);

            var item = new ToolBoxItem()
            {
                IsButton = true,
                Button = button,
            };

            _option.Items.Add(item);

            return button;
        }

        public ToolBoxRadioGroup AddRadioButton(string inputName)
        {
            var radioGroup = new ToolBoxRadioGroup()
            {
                RadioGroupName = inputName,
                RadioGroups = new List<ToolBoxRadio>()
            };

            var item = new ToolBoxItem()
            {
                IsRadioGroup = true,
                RadioGroup = radioGroup
            };

            _option.Items.Add(item);

            return radioGroup;
        }

        public void AddDataInfo(string text, string value)
        {
            var item = new ToolBoxItem()
            {
                IsDataInfo = true,
                DataInfoText = text,
                DataInfoValue = value,
            };

            _option.Items.Add(item);
        }

        
    }
}