using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Enum;

namespace NezamEquipment.Web.Areas.Admin.Features.Equipment
{
    public class AdminNezamEquipmentIndexSearchViewModel
    {

        [Display(Name = "نوع تجهیزات")]
        public EquipmentType? EquipmentType { get; set; }

        [Display(Name = "وضعیت")]
        public EquipmentStatus EquipmentStatus { get; set; }

        [Display(Name = "واحد")]
        public UnitType? UnitType { get; set; }

        [Display(Name = "کد اموال")]
        public string Code { get; set; }

        [Display(Name = "از تاریخ خرید")]
        public DateTime? FromCreatedOn { get; set; }

        [Display(Name = "تا تاریخ خرید")]
        public DateTime? ToCreatedOn { get; set; }
    }
}
