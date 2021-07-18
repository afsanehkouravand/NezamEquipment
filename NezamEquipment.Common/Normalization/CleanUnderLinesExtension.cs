using System.Globalization;

namespace NezamEquipment.Common.Normalization
{
    public static class CleanUnderLinesExtension
    {
        /// <summary>
        /// حذف آندرلاین در متن - underline ( _ )
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanUnderLines(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            const char chr1600 = (char)1600; //ـ=1600
            const char chr8204 = (char)8204; //‌=8204

            return text.Replace(chr1600.ToString(CultureInfo.InvariantCulture), "")
                       .Replace(chr8204.ToString(CultureInfo.InvariantCulture), "");
        }
    }
}
