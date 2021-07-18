using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.ServiceLayer.Entity.Equipment.Dto;

namespace NezamEquipment.Web.Areas.Admin.Features.Equipment
{
    public class AdminNezamEquipmentPartialFormViewModel
    {
        public EquipmentDto EquipmentDto { get; set; }
        [Display(Name = "انتخاب کارمند")]
        public IDictionary<string, string> DropDown { get; set; }


        [Display(Name = "رمز عبور")]
        public string PasswordChange { get; set; }

        [Display(Name = "تایید رمز عبور")]
        [Compare(nameof(PasswordChange))]
        public string ConfirmPassword { get; set; }


    }
}
