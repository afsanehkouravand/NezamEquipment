using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.EquipmentFaultyFile.Dto
{
    public class EquipmentFaultyFileDto : BaseEntityDto
    {

        /// <summary>
        /// نام فایل
        /// </summary>
        [Display(Name = "نام فایل")]     
        [Required, StringLength(50)]
        public string FileName { get; set; }

        /// <summary>
        /// پسوند فایل
        /// </summary>
        [Required, StringLength(10)]
        [Display(Name = "پسوند فایل")]
        public string Extension { get; set; }

        /// <summary>
        /// نام اصلی فایل
        /// </summary>
        [Required, StringLength(200)]
        [Display(Name = "نام اصلی فایل")]
        public string OriginalFileName { get; set; }

        public string FilePatch { get; set; }
        public Guid EquipmentFaultyId { get; set; }

        public int Type { get; set; }
     
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
