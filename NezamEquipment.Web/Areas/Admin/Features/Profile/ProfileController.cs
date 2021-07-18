using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Areas.Admin.Features.Profile;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Areas.Admin.Features.Profile
{
    [AuthorizeControllerName("پروفایل کاربری")]
    public partial class ProfileController : AdminBaseController
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public ProfileController(
            IUserManager userManager,
            IRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Index)]
        public virtual async Task<ActionResult> Index()
        {
            var id = _userManager.GetCurrentUserId();
            var user = await _userManager.GetAsync(id);
            if (user == null)
                return HttpNotFound();

            return View(new AdminProfileIndexViewModel()
            {
                User = user,
                Roles = await _roleManager.GetAllAsync(user.Id),
            });
        }

        #endregion

        #region Edit

        [AuthorizeActionName(AuthorizeActionNameAttribute.Title.Edit)]
        public virtual async Task<ActionResult> Edit()
        {
            var user = await _userManager.GetAsync(id: _userManager.GetCurrentUserId());
            if (user == null)
                return HttpNotFound();

            return View(new AdminProfileEditViewModel()
            {
                User = user,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Edit([Bind(Prefix = "User", Exclude = "Id")] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            var user = await _userManager.GetAsync(_userManager.GetCurrentUserId());
            if (user == null)
                return AjaxResult("کاربر مورد نظر یافت نشد.");

            userDto.Id = user.Id;
            userDto.UserName = user.UserName;
            userDto.Position = user.Position;

            var result = await _userManager.UpdateAsync(userDto);
            return AjaxResult(result);
        }

        #endregion

    }
}