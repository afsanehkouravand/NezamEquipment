using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.Common.Normalization;
using NezamEquipment.Common.ServiceLayer;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Areas.Admin.Features.User
{
    [AuthorizeControllerName("کاربران")]
    public partial class UserController : AdminBaseController
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public UserController(
            IUserManager userManager,
            IRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index(
            [Bind(Prefix = "S")]AdminUserIndexSearchViewModel s, int page = 1, int pageSize = 20, Guid? roleId = null)
        {
            s = s ?? new AdminUserIndexSearchViewModel();
            s.RoleId = roleId ?? s.RoleId;
            s.DropDownRole = (await _roleManager.GetAllAsync()).ToDictionary(x => x.Id.ToString(),
                x => x.Name);

            RoleDto role = null;
            if (s.RoleId != null)
            {
                role = await _roleManager.GetAsync(id: s.RoleId.Value);
                if (role == null)
                    return HttpNotFound();
            }

            var viewModel = new AdminUserIndexViewModel()
            {
                S = s,
                Role = role,
                PageSize = pageSize,
                PageNumber = (page <= 0) ? 1 : page,
            };

            var getAllTupleDto = GetAllTupleDto.Get(skip: viewModel.PageNumber, take: viewModel.PageSize);
            var data = await
                _userManager.GetAllWithRolesAsync(getAllTupleDto: getAllTupleDto, roleId: s.RoleId,
                    username: s.Username, areaType: s.AreaType, fullname: s.Fullname, position: s.Position);

            viewModel.Users = data.List;
            viewModel.PageTotal = data.Count;

            return View(viewModel);
        }

        #endregion

        #region Details

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Details)]
        public virtual async Task<ActionResult> Details(Guid id)
        {
            var user = await _userManager.GetAsync(id);
            if (user == null)
                return HttpNotFound();

            return View(new AdminUserDetailsViewModel()
            {
                User = user,
                Roles = await _roleManager.GetAllAsync(user.Id),
            });
        }

        #endregion

        #region Add

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Add)]
        public virtual async Task<ActionResult> Add()
        {
            var allRoles = await _roleManager.GetAllAsync();
            return View(new AdminUserAddViewModel()
            {
                PartialForm = new AdminUserPartialFormViewModel()
                {
                    AllRoles = allRoles.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Name
                    }),
                },
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Add([Bind(Prefix = "User", Exclude = "Id")] UserDto userDto, 
            IList<string> selectedRole)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            userDto.UserName = userDto.UserName.ApplyModeratePersianRulesForUserName();
            
            if (string.IsNullOrWhiteSpace(userDto.Password))
                return AjaxResult("لطفا یک رمز عبور وارد کنید.");

            var result = await _userManager.AddAsync(userDto, selectedRole);
            return result ? AjaxResult() : AjaxResult(GetMessage.CanNotChangeModel);
        }

        #endregion

        #region Edit

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Edit)]
        public virtual async Task<ActionResult> Edit(Guid id)
        {
            var user = await _userManager.GetAsync(id);
            if (user == null)
                return HttpNotFound();

            var allRolesForUser = await _roleManager.GetAllAsync(user.Id);
            var allRoles = await _roleManager.GetAllAsync();

            return View(new AdminUserEditViewModel()
            {
                PartialForm = new AdminUserPartialFormViewModel()
                {
                    User = user,
                    AllRoles = allRoles.Select(x => new SelectListItem
                    {
                        Selected = allRolesForUser.Select(c => c.Name).Contains(x.Name),
                        Text = x.Name,
                        Value = x.Name
                    }),
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit(
            [Bind(Prefix = "User")] UserDto userDto, IList<string> selectedRole)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            userDto.UserName = userDto.UserName.ApplyModeratePersianRulesForUserName();

            var result = await _userManager.UpdateAsync(userDto, selectedRole);
            return AjaxResult(result);
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Delete)]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            var result = await _userManager.DeleteAsync(id);
            return AjaxResult(result);
        }

        #endregion

    }
}