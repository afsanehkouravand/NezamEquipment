namespace MyCommon.Helpers.Grid
{
    public interface IPagingData
    {
        /// <summary>
        /// تعداد کل ردیف ها
        /// </summary>
        int PageTotal { get; set; }

        /// <summary>
        /// تعداد ردیف های نمایش داده شده در جدول
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// شماره صفحه
        /// </summary>
        int PageNumber { get; set; }
    }
}