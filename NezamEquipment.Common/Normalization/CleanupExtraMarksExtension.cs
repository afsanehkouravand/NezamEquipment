using System.Text.RegularExpressions;

namespace NezamEquipment.Common.Normalization
{
    public static class CleanupExtraMarksExtension
    {
        /// <summary>
        /// حذف بیش از یک مورد علامت تعجب و سئوال
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanupExtraMarks(this string text)
        {
            var phase1 = Regex.Replace(text, @"(!){2,}", "$1");
            var phase2 = Regex.Replace(phase1, "(؟){2,}", "$1");
            return phase2;
        }
    }
}
