using System.Text.RegularExpressions;

namespace NezamEquipment.Common.Normalization
{
    public static class ContainsFarsiExtension
    {
        /// <summary>
        /// بررسی وجود حروف و اعداد فارسی در متن
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool ContainsFarsiData(this string txt)
        {
            return !string.IsNullOrEmpty(txt) &&
                    Regex.IsMatch(txt, "[ا-یءئ]");
        }


        /// <summary>
        /// بررسی وجود حروف فارسی در متن
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool ContainsFarsi(this string txt)
        {
            return !string.IsNullOrEmpty(txt) &&
                    Regex.IsMatch(txt, @"[\u0600-\u06FF]");
        }
    }
}
