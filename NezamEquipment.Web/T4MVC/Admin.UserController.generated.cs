// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace NezamEquipment.Web.Areas.Admin.Features.User
{
    public partial class UserController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected UserController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Details()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Details);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Edit()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> Delete()
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Delete);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public UserController Actions { get { return MVC.Admin.User; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "User";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "User";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Details = "Details";
            public readonly string Add = "Add";
            public readonly string Edit = "Edit";
            public readonly string Delete = "Delete";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Details = "Details";
            public const string Add = "Add";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string s = "S";
            public readonly string page = "page";
            public readonly string pageSize = "pageSize";
            public readonly string roleId = "roleId";
        }
        static readonly ActionParamsClass_Details s_params_Details = new ActionParamsClass_Details();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Details DetailsParams { get { return s_params_Details; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Details
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_Add s_params_Add = new ActionParamsClass_Add();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Add AddParams { get { return s_params_Add; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Add
        {
            public readonly string userDto = "User";
            public readonly string selectedRole = "selectedRole";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string id = "id";
            public readonly string userDto = "User";
            public readonly string selectedRole = "selectedRole";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete
        {
            public readonly string id = "id";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_UserController : NezamEquipment.Web.Areas.Admin.Features.User.UserController
    {
        public T4MVC_UserController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, NezamEquipment.Web.Areas.Admin.Features.User.AdminUserIndexSearchViewModel s, int page, int pageSize, System.Guid? roleId);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Index(NezamEquipment.Web.Areas.Admin.Features.User.AdminUserIndexSearchViewModel s, int page, int pageSize, System.Guid? roleId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "S", s);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "pageSize", pageSize);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "roleId", roleId);
            IndexOverride(callInfo, s, page, pageSize, roleId);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void DetailsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Details(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Details);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DetailsOverride(callInfo, id);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void AddOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Add()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Add);
            AddOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void AddOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, NezamEquipment.ServiceLayer._Identity.User.Dto.UserDto userDto, System.Collections.Generic.IList<string> selectedRole);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> Add(NezamEquipment.ServiceLayer._Identity.User.Dto.UserDto userDto, System.Collections.Generic.IList<string> selectedRole)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Add);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "User", userDto);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "selectedRole", selectedRole);
            AddOverride(callInfo, userDto, selectedRole);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid id);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Edit(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            EditOverride(callInfo, id);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, NezamEquipment.ServiceLayer._Identity.User.Dto.UserDto userDto, System.Collections.Generic.IList<string> selectedRole);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> Edit(NezamEquipment.ServiceLayer._Identity.User.Dto.UserDto userDto, System.Collections.Generic.IList<string> selectedRole)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "User", userDto);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "selectedRole", selectedRole);
            EditOverride(callInfo, userDto, selectedRole);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

        [NonAction]
        partial void DeleteOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, System.Guid id);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.JsonResult> Delete(System.Guid id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Delete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DeleteOverride(callInfo, id);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.JsonResult);
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114