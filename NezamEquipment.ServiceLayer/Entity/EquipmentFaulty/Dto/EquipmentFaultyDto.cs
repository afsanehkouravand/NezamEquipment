using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.EquipmentFaulty.Dto
{
   
    public class EquipmentFaultyDto : BaseEntityDto
    {
        #region Properties

        /// <summary>
        /// تاریخ تعمیر
        /// </summary>
        [Display(Name = "تاریخ تعمیر")]
        [DataType(DataType.Date)]
        public DateTime RepairDate { get; set; }

        /// <summary>
        /// تاریخ تعمیر
        /// </summary>

        [Display(Name = "تاریخ تعمیر")]
        public string RepairDatestr { get; set; }

        /// <summary>
        /// هزینه تعمیر 
        /// </summary>
        [Display(Name = "هزینه تعمیر ")]
        public string MoneyRepair { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        [StringLength(5000)]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

      
        [Display(Name = "کاربر")]
        public Guid UserId { get; set; }

        public string UserFullName { get; set; }

        [Display(Name = "تجهیزات")]
        public Guid EquipmentId { get; set; }

     

        #endregion



    }
}
