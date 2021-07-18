using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Enum;

namespace NezamEquipment.Web.Areas.Admin.Features.Employees
{
    public class AdminNezamEmployeIndexSearchViewModel
    {

        [Display(Name = "نام و نام خانوادگی")]
        public string NezamEmployeFullName { get; set; }

        [Display(Name = "کد ملی")]
        public string NezamEmployeNationalCode { get; set; }

        [Display(Name = "نام واحد")]
        public UnitType? UnitType { get; set; }

        [Display(Name = "کد پرسنلی")]
        public int? CodePersonal { get; set; }
      
        [Display(Name = "IP")]
        public string  IP { get; set; }
        
        [Display(Name = "نوع شبکه")]
        public NetworkType? NetworkType { get; set; }
    }
}
