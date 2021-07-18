using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Models.NezamEmployeRegister
{
    public class RegisterMobileConfirmViewModel
    {
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = "کد تایید")]
        public string Code { get; set; }
        
        public bool MobileIsConfirmd { get; set; }
        public bool CaptchaValid { get; set; }
        
    }
}