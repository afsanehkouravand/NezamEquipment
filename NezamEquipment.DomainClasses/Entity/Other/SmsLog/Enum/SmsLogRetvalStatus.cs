using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum
{
    public enum SmsLogRetvalStatus
    {
        /// <summary>
        /// ارسال شده
        /// </summary>
        [Display(Name = "ارسال شده")]
        Sent = 0,

        /// <summary>
        /// ناموفق
        /// </summary>
        [Display(Name = "ناموفق")]
        Failed = 1,

        /// <summary>
        /// ناموفق
        /// </summary>
        [Display(Name = "منتظر ارسال")]
        Wait = 2,
    }
}
