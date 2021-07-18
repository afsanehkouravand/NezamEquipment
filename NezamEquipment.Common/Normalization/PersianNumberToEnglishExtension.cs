namespace NezamEquipment.Common.Normalization
{
    public static class PersianNumberToEnglishExtension
    {
        /// <summary>
        /// تبدیل اعداد فارسی به انگلیسی
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string PersianNumberToEnglish(this string data)
        {
            if (!string.IsNullOrEmpty(data))
                data = data.Trim();

            var numbers = new[] { "۰۱۲۳۴۵۶۷۸۹".ToCharArray(), "0123456789".ToCharArray() };
            for (var x = 0; x <= 9; x++)
            {
                data = data?.Replace(numbers[0][x], numbers[1][x]);
            }

            return data;
        }
    }
}
