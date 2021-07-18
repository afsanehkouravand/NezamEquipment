using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Equipment.Dto
{
   
    public class EquipmentDto : BaseEntityDto
    {
        #region Properties

        /// <summary>
        /// کد اموال
        /// </summary>
        [Display(Name = "کد اموال")]
        public string Code { get; set; }

        /// <summary>
        ///  برند
        /// </summary>
        [Display(Name = "برند")]
        public string Brand { get; set; }

        /// <summary>
        ///  مدل
        /// </summary>
        [Display(Name = "مدل")]
        public string Model { get; set; }


        /// <summary>
        ///   نوع تجهیزات
        /// </summary>
        [Display(Name = "نوع تجهیزات")]
        public EquipmentType EquipmentTypes { get; set; }

        /// <summary>
        ///   وضعیت
        /// </summary>
        [Display(Name = "وضعیت")]
        public EquipmentStatus EquipmentStatus { get; set; }


        /// <summary>
        /// تاریخ خرید
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "تاریخ خرید")]
        public DateTime? DateBuy { get; set; }
        /// <summary>
        /// تاریخ خرید
        /// </summary>
       
        [Display(Name = "تاریخ خرید")]
        public string DateBuystr { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [StringLength(5000)]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "واحد")]
        public UnitType unitType { get; set; }
       
        /// <summary>
        ///ای دی پرسنل
        /// </summary>
        [Display(Name = "پرسنل")]
        public Guid? EmployeesId { get; set; }
        [Display(Name = "پرسنل")]
        public string EmployeFullName { get; set; }
      
        [Display(Name = "کاربر")]
        public Guid UserId { get; set; }

        public string UserFullName { get; set; }


        #endregion



    }
}
