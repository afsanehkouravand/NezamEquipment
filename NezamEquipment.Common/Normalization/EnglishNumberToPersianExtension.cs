namespace NezamEquipment.Common.Normalization
{
    public static class EnglishNumberToPersianExtension
    {
        /// <summary>
        /// تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EnglishNumberToPersian(this string data)
        {
            if (!string.IsNullOrEmpty(data))
                data = data.Trim();

            var numbers = new[] { "0123456789".ToCharArray(), "۰۱۲۳۴۵۶۷۸۹".ToCharArray() };
            for (var x = 0; x <= 9; x++)
            {
                data = data?.Replace(numbers[0][x], numbers[1][x]);
            }

            return data;
        }
    }
}
