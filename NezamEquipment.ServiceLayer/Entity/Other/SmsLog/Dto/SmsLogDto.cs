using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.ServiceLayer.Base;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;

namespace NezamEquipment.ServiceLayer.Entity.Other.SmsLog.Dto
{
    public class SmsLogDto : BaseEntityDto
    {
        /// <summary>
        /// وضعیت
        /// </summary>
        [Display(Name = "وضعیت")]
        public SmsLogStatus SmsLogStatus { get; set; }

        /// <summary>
        /// وضعیت ارسال
        /// </summary>
        [Display(Name = "وضعیت ارسال")]
        public SmsLogRetvalStatus RetvalStatus { get; set; }

        /// <summary>
        /// کد رهگیری
        /// </summary>
        [Required, StringLength(20)]
        [Display(Name = "کد رهگیری")]
        public string RetvalNumber { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        [Required, StringLength(11), Index]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  متن پیامک
        /// </summary>
        [Required, StringLength(1000)]
        [Display(Name = "متن پیامک")]
        public string Message { get; set; }

        /// <summary>
        /// وضعیت دریافت
        /// </summary>
        [Display(Name = "وضعیت دریافت")]
        public SmsLogDeliveryStatus? DeliveryStatus { get; set; }

        /// <summary>
        /// تاریخ آخرین بررسی
        /// </summary>
        [Display(Name = "تاریخ آخرین بررسی")]
        public DateTime? DeliveryCheckDate { get; set; }
    }
}