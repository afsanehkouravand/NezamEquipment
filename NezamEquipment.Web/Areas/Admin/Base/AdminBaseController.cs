
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.Role.Enum;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Framework.Base;
using NezamEquipment.Web.Framework.Extension;

using NezamEquipment.Web.Framework.Security;
using NezamEquipment.Web.Models;
using System;
using System.Web.Mvc;

namespace NezamEquipment.Web.Areas.Admin.Base
{
    public partial class AdminBaseController : BaseController
    {
        public AdminBaseController(){}

        public HaveAccessToViewModel BaseHaveAccessTo { get; set; }
        public UserDto BaseUser { get; set; }
        public string BaseArea { get; set; }
        public string BaseController { get; set; }
        public string BaseAction { get; set; }

        #region OnActionExecuting

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseArea = filterContext.HttpContext.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            BaseController = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            BaseAction = filterContext.HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            var userManager = DependencyResolver.Current.GetService<IUserManager>();
            var roleManager = DependencyResolver.Current.GetService<IRoleManager>();

            var url = new UrlHelper(filterContext.RequestContext);

            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(url.Action(MVC.Admin.Account.Login()));
                return;
            }

            var userId = userManager.GetCurrentUserId();
            if (userId == Guid.Empty)
            {
                filterContext.Result = new RedirectResult(url.Action(MVC.Admin.Account.Login()));
                return;
            }

            BaseUser = userManager.Get(userManager.GetCurrentUserId());
            BaseHaveAccessTo = new HaveAccessToViewModel
            {
                IsAdmin = roleManager.IsInRole(userManager.GetCurrentUserId(),
                    roleManager.GetDefaultRole(DefaultRole.Admin)),
                CurrentController = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"]
                    .ToString(),
                CurrentArea = filterContext.HttpContext.Request.RequestContext.RouteData.DataTokens["area"].ToString(),
                RoleAccess = roleManager.GetAllControllersNameThatTheUserHaveAccessTo(userManager.GetCurrentUserId()),
            };

            #region Check HaveAccessTo

            var fullname = $"{filterContext.Controller.GetType().FullName}.{filterContext.ActionDescriptor.ActionName}";
            if (!NoneAuthorizeActionExtension.Check(fullname: fullname, fromCache: false))
            {
                if (!BaseHaveAccessTo.Check(area: BaseArea, controller: BaseController, action: BaseAction))
                {
                    filterContext.Result = new RedirectResult(url.Action(MVC.Home.Index()));
                    //return;
                }
            }

            #endregion

        }

        #endregion

        #region OnActionExecuted

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

          //  if (!System.Web.HttpContext.Current.Request.IsLocal)
           //     LogView(filterContext, BaseArea, BaseController, BaseAction, BaseUser.Id);

            //if (Request.IsAjaxRequest())
            //    return; 

            var viewResult = filterContext.Result as ViewResultBase;
            var model = viewResult?.ViewData.Model as AdminBaseViewModel;
            if (model != null)
            {
                model.HaveAccessTo = BaseHaveAccessTo;
                model.CurrentUser = BaseUser;

                model.LayoutTitle = GlobalInfo.LayoutTitle;
            }
        }

        #endregion

    }
}