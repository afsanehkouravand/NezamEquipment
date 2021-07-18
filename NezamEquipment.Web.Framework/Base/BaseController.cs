using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NezamEquipment.Common.Extension;
using NezamEquipment.ServiceLayer;
using NezamEquipment.Web.Framework.Result;

namespace NezamEquipment.Web.Framework.Base
{
    public class BaseController : Controller
    {

        #region AjaxResult

        protected JsonResult AjaxResult(bool isSucceed)
        {
            return Json(new AjaxResult(isSucceed: isSucceed, message: string.Empty), JsonRequestBehavior.AllowGet);
        }

        protected JsonResult AjaxResult()
        {
            return Json(new AjaxResult(isSucceed: true, message: string.Empty), JsonRequestBehavior.AllowGet);
        }

        protected JsonResult AjaxResult(bool isSucceed, string message)
        {
            return Json(new AjaxResult(isSucceed: isSucceed, message: message), JsonRequestBehavior.AllowGet);
        }


        protected JsonResult AjaxResult(DbResult dbResult)
        {
            return Json(new AjaxResult(isSucceed: dbResult.Status, message: dbResult.Message), JsonRequestBehavior.AllowGet);
        }

        protected JsonResult AjaxResult(string errorMessage)
        {
            return Json(new AjaxResult(isSucceed: false, message: errorMessage), JsonRequestBehavior.AllowGet);
        }

        protected JsonResult AjaxResult(GetMessage id)
        {
            var message = id.GetDisplayName();
            return Json(new AjaxResult(isSucceed: false, message: message), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region EnumGetMessage

        public enum GetMessage
        {
            [Display(Name = "")]
            None,

            /// <summary>
            /// خطایی در پردازش اطلاعات رخ داد.
            /// </summary>
            [Display(Name = "خطایی در پردازش اطلاعات رخ داد.")]
            Exception,

            /// <summary>
            /// اطلاعات ارسال شده صحیح نمی باشند.
            /// </summary>
            [Display(Name = "اطلاعات ارسال شده صحیح نمی باشند.")]
            ModelStateIsNotValid,

            /// <summary>
            /// چنین آیتمی پیدا نشد.
            /// </summary>
            [Display(Name = "چنین آیتمی پیدا نشد.")]
            CanNotFindModel,

            /// <summary>
            /// امکان ذخیره اطلاعات وجود ندارد."
            /// </summary>
            [Display(Name = "امکان ذخیره اطلاعات وجود ندارد.")]
            CanNotChangeModel,
        }

        #endregion


    }
}