using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NezamEquipment.DomainClasses.Base;
using NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum;


namespace NezamEquipment.DomainClasses.Entity.Other.SmsLog
{
    public class SmsLog : BaseEntity
    {
        /// <summary>
        /// وضعیت
        /// </summary>
        public SmsLogStatus SmsLogStatus { get; set; }

        /// <summary>
        /// وضعیت ارسال
        /// </summary>
        public SmsLogRetvalStatus RetvalStatus { get; set; }

        /// <summary>
        /// شماره رهگیری
        /// </summary>
        [Required, StringLength(20)]
        public string RetvalNumber { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        [Required, StringLength(11), Index]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///  متن پیامک
        /// </summary>
        [Required, StringLength(1000)]
        public string Message { get; set; }

        /// <summary>
        /// وضعیت دریافت
        /// </summary>
        public SmsLogDeliveryStatus? DeliveryStatus { get; set; }

        /// <summary>
        /// تاریخ آخرین بررسی
        /// </summary>
        public DateTime? DeliveryCheckDate { get; set; }

        public bool MustSend { get; set; }
    }
}