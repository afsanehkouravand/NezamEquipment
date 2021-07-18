namespace NezamEquipment.Common.Normalization
{
    public static class ApplyCorrectYeKeExtension
    {
        public const char ArabicYeChar = (char)1610;
        public const char PersianYeChar = (char)1740;

        public const char ArabicKeChar = (char)1603;
        public const char PersianKeChar = (char)1705;

        /// <summary>
        /// تصحیح حرف ی و ک در زبان عربی به فارسی
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ApplyCorrectYeKe(this string data)
        {
            return string.IsNullOrWhiteSpace(data) ?
                        string.Empty :
                        data.Replace(ArabicYeChar, PersianYeChar).Replace(ArabicKeChar, PersianKeChar).Trim();
        }

    }
}
