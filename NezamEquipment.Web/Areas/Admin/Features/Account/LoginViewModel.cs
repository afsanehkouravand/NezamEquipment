using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.Web.Areas.Admin.Features.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفا یک نا کاربری وارد کنید.")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

    }
}