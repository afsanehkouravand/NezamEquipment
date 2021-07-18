using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.DomainClasses.Entity.Equipment;
using NezamEquipment.DomainClasses.Identity;

namespace NezamEquipment.DomainClasses.Entity.EquipmentFaulty
{
    public class EquipmentFaulty : BaseEntity
    {
       
        #region Properties


        /// <summary>
        /// تاریخ تعمیر
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime RepairDate { get; set; }
        
        /// <summary>
        /// هزینه تعمیر 
        /// </summary>
        [StringLength(50)]
        public string MoneyRepair { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        [StringLength(5000)]
        public string Description { get; set; }
        
        /// <summary>
        ///user
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public Guid UserId { get; set; }

        /// <summary>
        ///ای دی تجهیزات
        /// </summary>
        [ForeignKey(nameof(EquipmentId))]
        public virtual Equipment.Equipment Equipment { get; set; }
        public Guid  EquipmentId { get; set; }
        public virtual ICollection<EquipmentFaultyFile> EquipmentFaultyFiles { get; set; }
        #endregion

        #region Navigation Properties

        // 
        #endregion

    }
}
