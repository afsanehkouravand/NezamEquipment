using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;
using NezamEquipment.DomainClasses.Enum;

namespace NezamEquipment.DomainClasses.Entity.EquipmentFaulty
{
    /// <summary>
    /// فایل های اجاره
    /// </summary>
    public class EquipmentFaultyFile : BaseEntity
    {
        /// <summary>
        /// نام فایل
        /// </summary>
        [Required, StringLength(50)]
        public string FileName { get; set; }

        /// <summary>
        /// پسوند فایل
        /// </summary>
        [Required, StringLength(10)]
        public string Extension { get; set; }

        /// <summary>
        /// نام اصلی فایل
        /// </summary>
        [Required, StringLength(200)]
        public string OriginalFileName { get; set; }
        
        /// <summary>
        ///  درخواست
        /// </summary>
        [ForeignKey(nameof(EquipmentFaultyId))]
        public virtual EquipmentFaulty EquipmentFaulty { get; set; }
        public Guid EquipmentFaultyId { get; set; }
        /// <summary>
        /// نوع فایل
        /// </summary>
        public EquipmentFaultyFileType FileType { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public EquipmentFaultyFileStatus Status { get; set; }
       


    }
}