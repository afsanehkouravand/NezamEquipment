using System.Globalization;
using System.Text;

namespace NezamEquipment.Common.Normalization
{
    public static class RemoveDiacriticsExtension
    {
        /// <summary>
        /// حذف اعراب از روی متن
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    //اسامی مانند آفتاب نباید خراب شوند
                    if (c == 1619) //آ
                    {
                        stringBuilder.Append(c);
                    }
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
