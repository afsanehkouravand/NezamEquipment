using System.ComponentModel.DataAnnotations;

namespace NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum
{
    public enum SmsLogStatus
    {
        /// <summary>
        /// نام کاربری یا رمزعبور اشتباه می باشد
        /// </summary>
        [Display(Name = "نام کاربری یا رمزعبور اشتباه می باشد")]
        InvalidUserPass = 0,

        /// <summary>
        /// درخواست با موفقیت انجام شد
        /// </summary>
        [Display(Name = "درخواست با موفقیت انجام شد")]
        Successfull = 1,

        /// <summary>
        /// اعتبار کافی نمی باشد
        /// </summary>
        [Display(Name = "اعتبار کافی نمی باشد")]
        NoCredit = 2,

        /// <summary>
        /// محدودیت در ارسال روزانه
        /// </summary>
        [Display(Name = "محدودیت در ارسال روزانه")]
        DailyLimit = 3,

        /// <summary>
        /// محدودیت در حجم ارسال
        /// </summary>
        [Display(Name = "محدودیت در حجم ارسال")]
        SendLimit = 4,

        /// <summary>
        /// شماره فرستنده معتبرنمی باشد
        /// </summary>
        [Display(Name = "شماره فرستنده معتبرنمی باشد")]
        InvalidNumber = 5,

        /// <summary>
        /// سامانه در حال بروزرسانی می باشد
        /// </summary>
        [Display(Name = "سامانه در حال بروزرسانی می باشد")]
        SystemIsDisable = 6,

        /// <summary>
        /// متن حاوی کلمه فیلتر شده می باشد
        /// </summary>
        [Display(Name = "متن حاوی کلمه فیلتر شده می باشد")]
        BadWords = 7,

        /// <summary>
        /// PardisMinimumReceivers
        /// </summary>
        [Display(Name = "PardisMinimumReceivers")]
        PardisMinimumReceivers = 8,

        /// <summary>
        /// ارسال از خطوط عمومی از طریق وب سرویس امکان پذیرنمی باشد
        /// </summary>
        [Display(Name = "ارسال از خطوط عمومی از طریق وب سرویس امکان پذیرنمی باشد")]
        NumberIsPublic = 9,



        /// <summary>
        /// منتظر ارسال
        /// </summary>
        [Display(Name = "منتظر ارسال")]
        Wait = 999,

    }
}