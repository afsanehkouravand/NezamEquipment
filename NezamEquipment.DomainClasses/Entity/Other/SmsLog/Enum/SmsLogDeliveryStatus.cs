namespace NezamEquipment.DomainClasses.Entity.Other.SmsLog.Enum
{
    public enum SmsLogDeliveryStatus
    {
        // ارسال شده به مخابرات
        PostedToContacts = 0,

        // رسیده به گوشی
        ReachedThePhone = 1,

        // نرسیده به گوشی
        NotReachingThePhone = 2,

        // خطای مخابراتی
        CommunicationError = 3,

        // خطای نا مشخص
        UnspecifiedError = 5,

        // رسیده به مخابرات
        ReachedToTelecommunications = 8,

        // نرسیده به مخابرات
        Unconfirmed = 16,

        // نا مشخص
        Unknown = 100,

        // ارسال شده
        Posted = 200,

        // فیلتر شده
        Filtered = 300,

        // در لیست ارسال
        InThePostingList = 400,

        // عدم پذیرش
        Rejection = 500,
    }
}