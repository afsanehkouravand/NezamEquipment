using System.Text.RegularExpressions;

namespace NezamEquipment.Common.Normalization
{
    public static class CleanUpZwnjExtension
    {
        /// <summary>
        /// Removes unnecessary zwnj char that are succeeded/preceded by a space
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanUpZwnj(this string text)
        {
            return Regex.Replace(text, @"\s+‌|‌\s+", " ");
        }

    }
}
