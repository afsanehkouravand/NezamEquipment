using System;
using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.ServiceLayer.Base
{
    public abstract class BaseEntityDto
    {
        public Guid Id { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public string CreatedOnStr { get; set; }

    }
}
