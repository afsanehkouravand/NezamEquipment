using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Enum;

namespace NezamEquipment.Web.Areas.Admin.Features.EquipmentFaulty
{
    public class AdminEquipmentFaultyIndexSearchViewModel
    {

       
        [Display(Name = "تاریخ تعمیر")]
      
        public DateTime RepairDate { get; set; }

       [Display(Name = "تاریخ تعمیر")]
        public string RepairDatestr { get; set; }

       
        [Display(Name = "هزینه تعمیر ")]
        public string MoneyRepair { get; set; }

        [Display(Name = "از تاریخ خرید")]
        public DateTime? FromCreatedOn { get; set; }

        [Display(Name = "تا تاریخ خرید")]
        public DateTime? ToCreatedOn { get; set; }
    }
}
