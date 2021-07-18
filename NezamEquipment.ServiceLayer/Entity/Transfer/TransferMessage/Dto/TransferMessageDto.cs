using System;
using System.ComponentModel.DataAnnotations;
using NezamEquipment.DomainClasses.Enum;
using NezamEquipment.ServiceLayer.Base;

namespace NezamEquipment.ServiceLayer.Entity.Transfer.TransferMessage.Dto
{
    /// <summary>
    /// تاریخچه پیام ها
    /// </summary>
    public class TransferMessageDto : BaseEntityDto
    {
        /// <summary>
        /// ای پی کاربر
        /// </summary>
        [StringLength(50)]
        public string ClientIp { get; set; }

        /// <summary>
        /// ای پی سیستم
        /// </summary>
        [StringLength(50)]
        public string MachineIp { get; set; }

        /// <summary>
        /// آدرس مک
        /// </summary>
        [StringLength(50)]
        public string MacAddress { get; set; }

        /// <summary>
        /// نوع مرورگر
        /// </summary>
        [StringLength(50)]
        public string Browser { get; set; }

        /// <summary>
        /// ایجنت
        /// </summary>
        public string Agent { get; set; }

        /// <summary>
        /// پیام
        /// </summary>
        [Required]
        [Display(Name = "پیام")]
        public string Message { get; set; }


        /// <summary>
        /// ارسال کننده
        /// </summary>
        [Display(Name = "ارسال کننده")]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// مراحل تایید
        /// </summary>
        [Display(Name = "مراحل تایید")]
        public TransferStep Step { get; set; }

        /// <summary>
        /// تایید
        /// </summary>
        [Display(Name = "تایید")]
        public bool Verification { get; set; }

        /// <summary>
        /// رد
        /// </summary>
        [Display(Name = "رد")]
        public bool Suspension { get; set; }

        /// <summary>
        /// ترخیص قطعی
        /// </summary>
        public Guid TransferId { get; set; }

        /// <summary>
        /// کاربر سازنده
        /// </summary>
        public Guid? CreatedByUserId { get; set; }

        [Display(Name = "نام و نام خانوادگی کارشناس")]
        public string CreatedByUserFullname { get; set; }
        [Display(Name = "سمت کارشناس")]
        public string CreatedByUserPosition { get; set; }

    }
}
