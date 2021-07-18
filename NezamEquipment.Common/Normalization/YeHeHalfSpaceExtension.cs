using System.Text.RegularExpressions;

namespace NezamEquipment.Common.Normalization
{
    public static class YeHeHalfSpaceExtension
    {
        /// <summary>
        /// Converts ه ی to ه‌ی
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string YeHeHalfSpace(this string text)
        {
            return Regex.Replace(text, @"(\S)(ه[\s‌]+[یي])(\s)", "$1ه‌ی‌$3"); // fix zwnj
        }
    }
}
