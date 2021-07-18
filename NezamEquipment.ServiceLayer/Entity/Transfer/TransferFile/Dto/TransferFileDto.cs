using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Entity.Transfer.Enum;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.TransferFile.Dto
{
    public class TransferFileDto : BaseEntityDto
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

        /// <summary>
        /// نوع فایل
        /// </summary>
        [Display(Name = "نوع فایل")]
        public TransferFileType FileType { get; set; }
        public string FileTypeStr { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        [Display(Name = "وضعیت")]
        public TransferFileStatus Status { get; set; }

        public string FilePatch { get; set; }
        public Guid TransferId { get; set; }

        public int Type { get; set; }

        /// <summary>
        ///وضعیت مجوز
        /// </summary>
        [Display(Name = "وضعیت مجوز")]
        public TransferStatus TransferStatus { get; set; }

        /// <summary>
        /// مراحل تایید
        /// </summary>
        [Display(Name = "مراحل تایید")]
        public TransferStep VerificationStep { get; set; }

        /// <summary>
        /// رد توسط کارشناس
        /// </summary>
        [Display(Name = "رد توسط کارشناس")]
        public bool IsSuspension { get; set; }


        /// <summary>
        /// ارسال مجدد توسط متقاضی
        /// </summary>
        [Display(Name = "ارسال مجدد توسط متقاضی")]
        public bool IsSuspensionReSended { get; set; }

    }
}
