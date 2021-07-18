using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Areas.Admin.Features.UserAccessList
{
    [AuthorizeControllerName("لیست دسترسی های کاربران")]
    public partial class UserAccessListController : AdminBaseController
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public UserAccessListController(
            IUserManager userManager,
            IRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index(
            string areaName = null, string controllerName = null, string actionName = null)
        {
            var user = await _userManager.GetAsync(id: _userManager.GetCurrentUserId());
            if (user == null)
                return HttpNotFound();

            var viewModel = new AdminUserAccessListIndexViewModel()
            {
                AllAreaWithController = GetAllAreaControllerAction.GetInstance().Tree,
                AreaName = areaName,
                ControllerName = controllerName,
                ActionName = actionName,
                RoleAccesses = await _roleManager.GetAllRoleAccessAsync(areaName,controllerName,actionName),
                Users = new List<UserDto>(),
            };

            var roleIds = viewModel.RoleAccesses.Select(x => x.RoleId).ToList().Distinct().ToList();
            if (roleIds.Any())
            {
                viewModel.Users = (await _userManager.GetAllWithRolesAsync(roleIds: roleIds)).List;
            }

            return View(viewModel);
        }

        #endregion

    }
}