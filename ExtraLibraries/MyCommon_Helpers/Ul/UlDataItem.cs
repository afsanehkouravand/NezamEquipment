using System.Threading.Tasks;
using System.Web.Mvc;
using MyCommon.Helpers.Extension;

namespace MyCommon.Helpers.Ul
{
    public class UlDataItem
    {
        private readonly UlOption _option;

        public UlDataItem(UlOption options)
        {
            _option = options;
        }

        public UlDataItem AddFor(string id, string title)
        {
            _option.Items.Add(new UlItem()
            {
                Id = id.ToLower(),
                Title = title,
                IsActive = true,
                AjaxUrl = string.Empty,
                AjaxHaveAccessToController = string.Empty,
                AjaxHaveAccessToAction = string.Empty,
                HaveAccessToArea = string.Empty,
            });
            return this;
        }
        public UlDataItem AddFor(string id, string title, ActionResult t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }
        public UlDataItem AddFor(string id, string title, Task<ActionResult> t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }
        public UlDataItem AddFor(string id, string title, JsonResult t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }
        public UlDataItem AddFor(string id, string title, Task<JsonResult> t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }

        public UlDataItem AddFor(EUlTabId id, string title)
        {
            _option.Items.Add(new UlItem()
            {
                Id = id.ToString().ToLower(),
                Title = title,
                IsActive = true,
                AjaxUrl = string.Empty,
                AjaxHaveAccessToController = string.Empty,
                AjaxHaveAccessToAction = string.Empty,
                HaveAccessToArea = string.Empty,
            });
            return this;
        }
        public UlDataItem AddFor(EUlTabId id, string title, ActionResult t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToString().ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }
        public UlDataItem AddFor(EUlTabId id, string title, Task<ActionResult> t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToString().ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }
        public UlDataItem AddFor(EUlTabId id, string title, JsonResult t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToString().ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }
        public UlDataItem AddFor(EUlTabId id, string title, Task<JsonResult> t4MvcActionUrl, bool? isActive = false)
        {
            var info = t4MvcActionUrl.UrlInfo(_option.RequestContext);

            _option.Items.Add(new UlItem()
            {
                Id = id.ToString().ToLower(),
                Title = title,
                IsActive = isActive ?? false,
                AjaxUrl = info.Url,
                AjaxHaveAccessToController = info.Controller,
                AjaxHaveAccessToAction = info.Action,
                HaveAccessToArea = info.Area,
            });
            return this;
        }

    }
}