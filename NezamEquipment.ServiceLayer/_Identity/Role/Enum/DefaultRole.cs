using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.ServiceLayer._Identity.Role.Enum
{
    public enum DefaultRole
    {
        [Display(Name = "ادمین")]
        Admin,

        [Display(Name = "کاربر اخراجی")]
        Banned,
        [Display(Name = "ارزیاب")]
        Arzyabi,
    }
}