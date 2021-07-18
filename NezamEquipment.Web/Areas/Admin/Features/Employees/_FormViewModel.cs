using System.ComponentModel.DataAnnotations;
using NezamEquipment.ServiceLayer.Entity.Employees.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.Employees
{
    public class AdminNezamEmployePartialFormViewModel
    {
        public EmployeDto EmployeesDto { get; set; }
    
        [Display(Name = "رمز عبور")]
        public string PasswordChange { get; set; }

        [Display(Name = "تایید رمز عبور")]
        [Compare(nameof(PasswordChange))]
        public string ConfirmPassword { get; set; }


    }
}
