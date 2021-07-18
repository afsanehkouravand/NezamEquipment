using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.Role.Dto;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;
using NezamEquipment.Web.Framework.Security;

namespace NezamEquipment.Web.Areas.Admin.Features.Role
{
    [AuthorizeControllerName("نقش ها")]
    public partial class RoleController : AdminBaseController
    {
        private readonly IRoleManager _roleManager;
        private readonly IUserManager _userManager;

        public RoleController(
            IRoleManager roleManager, 
            IUserManager userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index()
        {
            var viewModel = new AdminRoleIndexViewModel()
            {
                Roles = await _roleManager.GetAllAsync(),
                DefaultRoles = await _roleManager.GetAllDefaultRoleAsync(),
            };

            foreach (var item in viewModel.Roles)
            {
                if (viewModel.DefaultRoles.Any(x=>x.Id == item.Id))
                {
                    item.IsDefaultRole = true;
                }
            }

            return View(viewModel);
        }

        #endregion

        #region Add

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Add)]
        public virtual ActionResult Add()
        {
            var viewModel = new AdminRoleAddViewModel()
            {
                PartialForm = new AdminRolePartialFormViewModel()
                {
                    AllAreaWithController = GetAllAreaControllerAction.GetInstance().Tree.Where(x => x.Name != "Store").ToList(),
                    ListOfAccesses = new List<RoleAccessDto>()
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Add(
            [Bind(Prefix = "Role", Exclude = "Id")]RoleDto model, List<RoleAccessDto> roleAccess)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            if (roleAccess == null || roleAccess.All(x => x.HaveAccess == null))
                return AjaxResult("هیچ دسسترسی برای نقش مذکور انتخاب نشده است.");

            var duplicate = new List<RoleAccessDto>();
            foreach (var item in roleAccess)
            {
                var dup = GetAllAreaControllerAction.GetInstance()
                    .List.FirstOrDefault(x => x.AreaName == item.Area && x.ControllerName == item.Controller &&
                                              x.IsDuplicateTo == item.Action);
                if (dup != null)
                {
                    duplicate.Add(new RoleAccessDto()
                    {
                        Action = dup.ActionName,
                        Controller = item.Controller,
                        Area = item.Area,
                        RoleId = item.RoleId,
                        Id = item.Id,
                        CreatedOn = item.CreatedOn,
                        CreateById = item.CreateById,
                        CreateByUserName = item.CreateByUserName,
                        HaveAccess = item.HaveAccess,
                        RoleName = item.RoleName,
                    });
                }
            }
            roleAccess.AddRange(duplicate);

            var check = await _roleManager.GetAsync(name: model.Name);
            if (check != null)
                return AjaxResult("این نقش از قبل ثبت شده است.");

            var result = await _roleManager.AddAsync(model);
            if (result.Status == false)
                return AjaxResult(result);

            var getNewRole = await _roleManager.GetAsync(model.Name);
            var userId = _userManager.GetCurrentUserId();

            var resultAccess = await _roleManager.AddRoleAccessAsync(userId: userId, roleId: getNewRole.Id, roleAccessDtos: roleAccess);
            return AjaxResult(resultAccess);
        }

        #endregion

        #region Edit

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Edit)]
        public virtual async Task<ActionResult> Edit(Guid id)
        {
            var role = await _roleManager.GetAsync(id: id);
            if (role == null)
                return HttpNotFound();

            if (_roleManager.IsDefaultRole(role.Name))
                return View(MVC.Admin.Features.Views.Role.NoEditForDefaultRole, new AdminRolePartialFormViewModel());

            var viewModel = new AdminRoleEditViewModel()
            {
                PartialForm = new AdminRolePartialFormViewModel()
                {
                    Role = role,
                    AllAreaWithController = GetAllAreaControllerAction.GetInstance().Tree.Where(x => x.Name != "Store").ToList(),
                    ListOfAccesses = await _roleManager.GetAllRoleAccessAsync(role.Id)
                }
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit(
            [Bind(Prefix = "Role")]RoleDto model, List<RoleAccessDto> roleAccess)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            if (roleAccess == null || roleAccess.All(x => x.HaveAccess == null))
                return AjaxResult("هیچ دسسترسی یرای نقش مذکور انتخاب نشده است.");

            var duplicate = new List<RoleAccessDto>();
            foreach (var item in roleAccess)
            {
                var dup = GetAllAreaControllerAction.GetInstance()
                    .List.FirstOrDefault(x => x.AreaName == item.Area && x.ControllerName == item.Controller &&
                                              x.IsDuplicateTo == item.Action);
                if (dup != null)
                {
                    duplicate.Add(new RoleAccessDto()
                    {
                        Action = dup.ActionName,
                        Controller = item.Controller,
                        Area = item.Area,
                        RoleId = item.RoleId,
                        Id = item.Id,
                        CreatedOn = item.CreatedOn,
                        CreateById = item.CreateById,
                        CreateByUserName = item.CreateByUserName,
                        HaveAccess = item.HaveAccess,
                        RoleName = item.RoleName,
                    });
                }
            }
            roleAccess.AddRange(duplicate);

            try
            {
                var role = await _roleManager.GetAsync(id: model.Id);
                if (role == null)
                    return AjaxResult(GetMessage.CanNotChangeModel);

                if (_roleManager.IsDefaultRole(role.Name))
                    return AjaxResult("امکان ویرایش نقش های پیش فرض وجود ندارد.");

                var result = await _roleManager.UpdateAsync(model);
                if (result.Status == false)
                    return AjaxResult(result);

                await _roleManager.DeleteRoleAccessAsync(model.Id);
                var userId = _userManager.GetCurrentUserId();

                var resultAccess = await _roleManager.AddRoleAccessAsync(userId: userId, roleId: model.Id, roleAccessDtos: roleAccess);

                return AjaxResult(resultAccess);
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
                return AjaxResult(GetMessage.Exception);
            }
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Delete)]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                var role = await _roleManager.GetAsync(id: id);
                if (role == null)
                    return AjaxResult(GetMessage.CanNotFindModel);

                if (_roleManager.IsDefaultRole(role.Name))
                    return AjaxResult("امکان حذف نقش های پیش فرض وجود ندارد.");

                var result = await _roleManager.DeleteAsync(role.Id);
                return AjaxResult(result);
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
                return AjaxResult(GetMessage.Exception);
            }
        }

        #endregion

    }
}