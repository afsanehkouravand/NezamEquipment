using System.Threading.Tasks;
using System.Web.Mvc;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting;
using NezamEquipment.ServiceLayer.Entity.Setting.Setting.Xml;
using NezamEquipment.Web.Areas.Admin.Base;
using NezamEquipment.Web.Framework.Attribute;

namespace NezamEquipment.Web.Areas.Admin.Features.Setting
{
    [AuthorizeControllerName("تنظیمات")]
    public partial class SettingController : AdminBaseController
    {
        private readonly ISettingService _settingService;

        public SettingController(
            ISettingService settingService)
        {
            _settingService = settingService;
        }

        #region Sms

        [AuthorizeActionName("تنظیمات پیامک ها")]
        public virtual async Task<ActionResult> Sms()
        {
            var viewModel = new AdminSettingSmsViewModel()
            {
                SettingSmsXml = await _settingService.GetAsync<SettingSmsXml>(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> Sms([Bind(Prefix = "SettingSmsXml")]SettingSmsXml model)
        {
            if (!ModelState.IsValid)
                return AjaxResult();

            var result = await _settingService.ModifyAsync(model);
            return AjaxResult(result);
        }

        #endregion


        #region AsanPardakht

        // GET: Admin/Mail
        [AuthorizeActionName("تنظیمات درگاه آسان پرداخت")]
        public virtual ActionResult AsanPardakht()
        {
            return View(new AdminSettingAsanPardakhtViewModel()
            {
                SettingAsanPardakhtXml = _settingService.Get<SettingAsanPardakhtXml>()
            });
        }

        // POST: Admin/AppSetting
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> AsanPardakht(SettingAsanPardakhtXml model)
        {
            if (!ModelState.IsValid)
                return AjaxResult(GetMessage.ModelStateIsNotValid);

            var result = await _settingService.ModifyAsync(model);
            return result.Status ? AjaxResult() : AjaxResult(GetMessage.CanNotChangeModel);
        }
        #endregion
    }
}