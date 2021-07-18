using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Grid
{
    public class GridDataOptions
    {
        private readonly GridOption _option;
        private readonly HtmlHelper _htmlHelper;

        public GridDataOptions(GridOption option, HtmlHelper htmlHelper)
        {
            _option = option;
            _htmlHelper = htmlHelper;
        }

        public void SetTitle(string title)
        {
            _option.TableTitle = title;
        }
        public void SetColor(EGridColorType color)
        {
            _option.TableColor = color;
        }
        public void HideFooterIfEmpty()
        {
            _option.HideFooterIfEmpty = true;
        }
        public void SetRowColorRoles(string propertyName,
            IDictionary<string, EGridColorType> colorRoles)
        {
            _option.IsRowColorRoles = true;
            _option.RowColorRolesPropertyName = propertyName;
            _option.RowColorRoles = colorRoles;
        }
        public void SetHaveAccessTo(object haveAccessTo = null, bool enabled = true)
        {
            _option.EnabledHaveAccessTo = enabled;
            _option.HaveAccessTo = haveAccessTo;
            _option.HaveAccessToRoleAccess = new List<RoleAccessDto>();

            if (haveAccessTo == null)
                return;
            
            var haveAccessToType = haveAccessTo.GetType();

            var propertyInfoIsAdmin = haveAccessToType.GetProperty("IsAdmin");
            if (propertyInfoIsAdmin != null)
            {
                var isAdmin = propertyInfoIsAdmin.GetValue(haveAccessTo, null);
                _option.HaveAccessToIsAdmin = isAdmin != null && bool.Parse(isAdmin.ToString());
            }

            var propertyInfoRoleAccess = haveAccessToType.GetProperty("RoleAccess");
            if (propertyInfoRoleAccess != null)
            {
                var roleAccess = propertyInfoRoleAccess.GetValue(haveAccessTo, null);
                if (roleAccess != null)
                {
                    var roleAccessCast = (IList) roleAccess;
                    foreach (var item in roleAccessCast)
                    {
                        var a = item.CustomCastToRoleAccess();
                        _option.HaveAccessToRoleAccess.Add(a);
                    }
                }
            }

        }
        public void SetEmptyResultMessage(string message = "", bool show = true)
        {
            _option.ShowEmptyResult = show;
            _option.EmptyResultMessage = message;
        }
        public void SetPaging(bool enabled, bool enabledPageSize = true)
        {
            _option.EnablePaging = enabled;
            _option.EnablePageSize = enabledPageSize;
        }

        #region Buttons

        public GridButton AddDetailsButton()
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new GridButton
            {
                TagType = "a",
                Text = "اطلاعات",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "label label-info" },
                    {"style", "margin:1px;" },
                },
                Href = url.Action("Details", controllerName, new { area = areaName }),
                HaveAccessToAction = "Details",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-info",
                QureyStrings = new List<GridButtonQueryString>()
                {
                    new GridButtonQueryString()
                    {
                        Key = "id",
                        Value = "Id"
                    }
                },
            };

            _option.Buttons.Add(button);

            return button;
        }
        public GridButton AddDetailsButton(ActionResult t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        public GridButton AddDetailsButton(Task<ActionResult> t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        public GridButton AddDetailsButton(JsonResult t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        public GridButton AddDetailsButton(Task<JsonResult> t4MvcActionUrl, string text = "اطلاعات")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDetailsButton(text, info);
        }
        private GridButton GetDetailsButton(string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                TagType = "a",
                Text = text,
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                IconClass = "fa fa-info",
                Href = info.Url,
            };

            if (!button.HtmlAttributes.ContainsKey("class"))
            {
                button.HtmlAttributes.Add("class", "label label-info");
            }
            if (!button.HtmlAttributes.ContainsKey("style"))
            {
                button.HtmlAttributes.Add("style", "margin:1px;");
            }

            _option.Buttons.Add(button);

            return button;
        }

        public GridButton AddEditButton()
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new GridButton
            {
                TagType = "a",
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
                QureyStrings = new List<GridButtonQueryString>()
                {
                    new GridButtonQueryString()
                    {
                        Key = "id",
                        Value = "Id"
                    }
                },
            };

            _option.Buttons.Add(button);

            return button;
        }
        public GridButton AddEditButton(ActionResult t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public GridButton AddEditButton(Task<ActionResult> t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public GridButton AddEditButton(JsonResult t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        public GridButton AddEditButton(Task<JsonResult> t4MvcActionUrl, string text = "ویرایش")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetEditButton(text, info);
        }
        private GridButton GetEditButton(string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                TagType = "a",
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

            _option.Buttons.Add(button);

            return button;
        }

        public GridButton AddDeleteButton()
        {
            var areaName = _htmlHelper.ViewContext.RouteData.DataTokens["area"].ToString();
            var controllerName = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString();

            var url = new UrlHelper(_htmlHelper.ViewContext.RequestContext);

            var button = new GridButton
            {
                TagType = "a",
                Text = "حذف",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"class", "label label-danger" },
                    {"style", "margin:1px;" },
                    {"data-delete", "true" },
                    {"data-id", "[Id]" },
                    {"data-url", url.Action("Delete", controllerName, new { area = areaName }) },
                },
                HaveAccessToAction = "Delete",
                HaveAccessToController = controllerName,
                HaveAccessToArea = areaName,
                IconClass = "fa fa-remove",
            };

            _option.Buttons.Add(button);

            return button;
        }
        public GridButton AddDeleteButton(ActionResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(text, info);
        }
        public GridButton AddDeleteButton(Task<ActionResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(text, info);
        }
        public GridButton AddDeleteButton(JsonResult t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(text, info);
        }
        public GridButton AddDeleteButton(Task<JsonResult> t4MvcActionUrl, string text = "حذف")
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetDeleteButton(text, info);
        }
        private GridButton GetDeleteButton(string text, GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                TagType = "a",
                Text = text,
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                IconClass = "fa fa-remove",
                HtmlAttributes = new Dictionary<string, string>()
                {
                    {"data-delete", "true" },
                    {"data-id", "[Id]" },
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

            _option.Buttons.Add(button);

            return button;
        }

        public GridButton AddButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public GridButton AddButton(string url)
        {
            var info = new GetUrlInfoExtension.GetUrlInfo()
            {
                Area = string.Empty,
                Controller = string.Empty,
                Action = string.Empty,
                Url = url,
            };
            return GetButton(info);
        }
        private GridButton GetButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
                TagType = "a",
            };

            _option.Buttons.Add(button);

            return button;
        }

        public GridButton AddAjaxButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public GridButton AddAjaxButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public GridButton AddAjaxButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        public GridButton AddAjaxButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetAjaxButton(info);
        }
        private GridButton GetAjaxButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                TagType = "a",
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

            _option.Buttons.Add(button);

            return button;
        }


        public GridButton AddPopupButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        public GridButton AddPopupButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        public GridButton AddPopupButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        public GridButton AddPopupButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetPopupButton(info);
        }
        private GridButton GetPopupButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new GridButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                TagType = "a",
            };

            button.HtmlAttributes.Add("data-popup", "true");
            button.HtmlAttributes.Add("data-url", info.Url);
            button.HtmlAttributes.Add("data-title", button.Text);

            _option.Buttons.Add(button);

            return button;
        }

        #endregion

        #region Form

        public GridForm AddForm(ActionResult t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            return GetForm(info);
        }
        public GridForm AddForm(Task<ActionResult> t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            return GetForm(info);
        }
        public GridForm AddForm(JsonResult t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            return GetForm(info);
        }
        public GridForm AddForm(Task<JsonResult> t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            return GetForm(info);
        }
        private GridForm GetForm(GetUrlInfoExtension.GetUrlInfo info)
        {
            _option.Form = new GridForm()
            {
                IsSetForm = true,
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Url = info.Url,
                OnBegin = "OnBegin",
                OnSuccess = "OnSuccess",
                OnComplete = "OnComplete",
                OnFailure = "OnFailure",
                UpdateTarget = "",
            };

            return _option.Form;
        }

        #endregion

        public void HideHeaderTag(bool hide = true)
        {
            _option.HideHeaderTag = hide;
        }

    }
}