using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Form
{
    public class FormDataOption
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly FormOption _option;

        public void IsSearchForm(ActionResult url, long pageTotal)
        {
            Url(url);
            SetSubmitSearch();
            SetIsComplex("S");
            DataInfoPageTotal(pageTotal);
            _option.FormItems.Add(new FormItem()
            {
                Tag = "legend",
                Class = "margin-bottom-10",
                ColSize = 12,
                Text = "جستجو",
            });
        }
        public void IsSearchForm(Task<ActionResult> url, long pageTotal)
        {
            Url(url);
            SetSubmitSearch();
            SetIsComplex("S");
            DataInfoPageTotal(pageTotal);
            _option.FormItems.Add(new FormItem()
            {
                Tag = "legend",
                Class = "margin-bottom-10",
                ColSize = 12,
                Text = "جستجو",
            });
        }

        public void IsAddForm(JsonResult url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            AddForm(info, gotopage, hiddenProperty);
        }
        public void IsAddForm(Task<JsonResult> url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            AddForm(info, gotopage, hiddenProperty);
        }
        public void IsAddForm(ActionResult url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            AddForm(info, gotopage, hiddenProperty);
        }
        public void IsAddForm(Task<ActionResult> url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            AddForm(info, gotopage, hiddenProperty);
        }
        private void AddForm(GetUrlInfoExtension.GetUrlInfo info, string gotopage, params string[] hiddenProperty)
        {
            _option.Url = info.Url;
            _option.HaveAccessToArea = info.Area;
            _option.HaveAccessToController = info.Controller;
            _option.HaveAccessToAction = info.Action;

            SetSubmit(EFormColorType.Success, "ثبت اطلاعات");
            SetAjaxData();
            SetGoToPage(gotopage);
            UseModelValue(false);

            if (hiddenProperty != null)
            {
                foreach (var item in hiddenProperty)
                {
                    SetHiddenInput(item.ToLower(), item);
                }
            }
        }

        public void IsEditForm(JsonResult url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            EditForm(info, gotopage, hiddenProperty);
        }
        public void IsEditForm(Task<JsonResult> url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            EditForm(info, gotopage, hiddenProperty);
        }
        public void IsEditForm(ActionResult url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            EditForm(info, gotopage, hiddenProperty);
        }
        public void IsEditForm(Task<ActionResult> url, string gotopage, params string[] hiddenProperty)
        {
            var info = url.UrlInfo(_option.RequestContext);
            EditForm(info, gotopage, hiddenProperty);
        }
        private void EditForm(GetUrlInfoExtension.GetUrlInfo info, string gotopage, params string[] hiddenProperty)
        {
            _option.Url = info.Url;
            _option.HaveAccessToArea = info.Area;
            _option.HaveAccessToController = info.Controller;
            _option.HaveAccessToAction = info.Action;

            SetSubmit(EFormColorType.Success, "ویرایش اطلاعات");
            SetAjaxData();
            SetGoToPage(gotopage);

            if (hiddenProperty != null)
            {
                foreach (var item in hiddenProperty)
                {
                    SetHiddenInput(item.ToLower(), item);
                }
            }
        }

        public FormDataOption(FormOption options, HtmlHelper htmlHelper)
        {
            _option = options;
            _htmlHelper = htmlHelper;
        }

        public void SetHaveAccessTo(object haveAccessTo, bool enabled = true)
        {
            _option.EnabledHaveAccessTo = enabled;
            _option.HaveAccessTo = haveAccessTo;
            _option.HaveAccessToIsAdmin = false;
            _option.HaveAccessToRoleAccess = new List<RoleAccessDto>();

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
                    var roleAccessCast = (IList)roleAccess;
                    foreach (var item in roleAccessCast)
                    {
                        _option.HaveAccessToRoleAccess.Add(item.CustomCastToRoleAccess());
                    }
                }
            }

        }

        public void SetAjaxData(string ajaxBegin = "OnBegin", string ajaxComplete = "OnComplete",
            string ajaxFailure = "OnFailure", string ajaxSuccess = "OnSuccess", string ajaxLoading = "loading",
            string updateTarget = null)
        {
            _option.UpdateTarget = updateTarget;
            _option.OnBegin = ajaxBegin;
            _option.OnComplete = ajaxComplete;
            _option.OnFailure = ajaxFailure;
            _option.OnSuccess = ajaxSuccess;
            _option.Loading = ajaxLoading;
            _option.IsAjaxForm = true;
            _option.FormMethod = EFormMethodType.Post;
        }

        public void IsDisabled(bool disabled)
        {
            _option.IsDisabled = disabled;
        }

        public void SetGoToPage(string goToPage)
        {
            _option.GoToPage = goToPage;
        }

        public void SetHiddenInput(string key, string value)
        {
            _option.HiddenProperties.Add(key, value);
        }

        public void SetIsComplex(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                _option.IsComplex = true;
                _option.Prefix = prefix;
            }
        }

        public void InsidePanelDiv(string title = "", EFormColorType color = EFormColorType.Default)
        {
            _option.InsidePanelDiv = true;
            _option.InsidePanelDivColor = color;
            _option.InsidePanelDivTitle = title;
        }

        public void SetSubmit(EFormColorType color, string text, bool? isSubmit = null, string onclick = null, string @class = null,
            string style = null)
        {
            _option.SubmitColor = color;
            _option.SubmitText = text;
            if (isSubmit != null)
            {
                _option.IsSubmit = isSubmit.Value;
            }
            if (!string.IsNullOrWhiteSpace(onclick))
            {
                _option.SubmitOnClick = onclick;
            }

            if (!string.IsNullOrWhiteSpace(@class))
            {
                _option.SubmitAttributes.Add("class", @class);
            }
            if (!string.IsNullOrWhiteSpace(style))
            {
                _option.SubmitAttributes.Add("style", style);
            }
        }

        public void SetSubmitSearch(EFormColorType color = EFormColorType.Default, string text = "جستجو")
        {
            _option.SubmitColor = color;
            _option.SubmitText = text;
        }

        public void UseModelValue(bool use)
        {
            _option.UseModelValue = use;
        }

        public void Url(ActionResult t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            FormUrl(info);
        }
        public void Url(Task<ActionResult> t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            FormUrl(info);
        }
        public void Url(JsonResult t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            FormUrl(info);
        }
        public void Url(Task<JsonResult> t4MvcActionResult)
        {
            var info = t4MvcActionResult.UrlInfo(_option.RequestContext);
            FormUrl(info);
        }
        private void FormUrl(GetUrlInfoExtension.GetUrlInfo info)
        {
            _option.Url = info.Url;
            _option.HaveAccessToArea = info.Area;
            _option.HaveAccessToController = info.Controller;
            _option.HaveAccessToAction = info.Action;
        }

        public void Method(EFormMethodType type)
        {
            _option.FormMethod = type;
        }

        public void DataInfo(string text, string value)
        {
            _option.DataInfos.Add(text, value);
        }

        public void DataInfoPageTotal(long total, string title = "تعداد رکوردها")
        {
            _option.DataInfos.Add(title, total.ToString());
        }

        public void IsMaterialStyle(bool materialStyle = true)
        {
            _option.IsMaterialStyle = materialStyle;
        }

        public void HideFormWell(bool hide = true)
        {
            _option.HideFormWell = hide;
        }

        public void SetClass(string @class)
        {
            _option.CssClass = @class;
        }


        public FormButton AddButton(string t4MvcActionUrl)
        {
            return GetButton(new GetUrlInfoExtension.GetUrlInfo()
            {
                Url = t4MvcActionUrl
            });
        }
        public FormButton AddButton(ActionResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public FormButton AddButton(Task<ActionResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public FormButton AddButton(JsonResult t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        public FormButton AddButton(Task<JsonResult> t4MvcActionUrl)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);
            return GetButton(info);
        }
        private FormButton GetButton(GetUrlInfoExtension.GetUrlInfo info)
        {
            var button = new FormButton
            {
                HaveAccessToAction = info.Action,
                HaveAccessToController = info.Controller,
                HaveAccessToArea = info.Area,
                Href = info.Url,
            };

            _option.FormButtons.Add(button);

            return button;
        }

    }
}