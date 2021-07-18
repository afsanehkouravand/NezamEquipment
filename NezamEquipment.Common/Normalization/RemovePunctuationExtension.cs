using System.Linq;

namespace NezamEquipment.Common.Normalization
{
    public static class RemovePunctuationExtension
    {
        /// <summary>
        /// حذف نقطه گذاری در متن
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemovePunctuation(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? string.Empty : new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
        }
    }
}
