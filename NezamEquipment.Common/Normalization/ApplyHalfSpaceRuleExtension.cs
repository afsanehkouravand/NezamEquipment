using System.Text.RegularExpressions;

namespace NezamEquipment.Common.Normalization
{
    public static class ApplyHalfSpaceRuleExtension
    {
        /// <summary>
        /// Adds zwnj char between word and prefix/suffix
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ApplyHalfSpaceRule(this string text)
        {
            //put zwnj between word and prefix (mi* nemi*)
            var phase1 = Regex.Replace(text, @"\s+(ن?می)\s+", @" $1‌");

            //put zwnj between word and suffix (*tar *tarin *ha *haye)
            var phase2 = Regex.Replace(phase1, @"\s+(تر(ی(ن)?)?|ها(ی)?)\s+", @"‌$1 ");
            return phase2;
        }
    }
}
