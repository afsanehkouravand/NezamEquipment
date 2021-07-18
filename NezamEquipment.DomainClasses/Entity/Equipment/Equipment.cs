using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;
using NezamEquipment.DomainClasses.Entity.Employees;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.DomainClasses.Identity;

namespace NezamEquipment.DomainClasses.Entity.Equipment
{
    public class Equipment : BaseEntity
    {

        #region Properties
        
        /// <summary>
        /// کد اموال
        /// </summary>
       
        public string Code { get; set; }

        /// <summary>
        ///  برند
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        ///  مدل
        /// </summary>
        public string Model { get; set; }

     
        /// <summary>
        ///   نوع اموال
        /// </summary>
        public EquipmentType EquipmentTypes { get; set; }

        /// <summary>
        ///   وضعیت
        /// </summary>
        public EquipmentStatus EquipmentStatus { get; set; }
                
        /// <summary>
        /// تاریخ خرید
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? DateBuy { get; set; }
        
        /// <summary>
        /// توضیحات
        /// </summary>
        [StringLength(5000)]
        public string Description { get; set; }

        public UnitType UnitType { get; set; }
        /// <summary>
        ///ای دی پرسنل
        /// </summary>
        [ForeignKey(nameof(EmployeesId))]
        public virtual Employe Employe { get; set; }
        public Guid? EmployeesId { get; set; }

        /// <summary>
        ///user
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        
        #endregion

        #region Navigation Properties
        public virtual ICollection<EquipmentFaulty.EquipmentFaulty> EquipmentFaulties { get; set; }

        #endregion

    }
}
