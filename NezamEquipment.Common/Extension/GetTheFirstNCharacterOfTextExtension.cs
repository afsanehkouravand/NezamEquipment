using System;
using System.Text;

namespace NezamEquipment.Common.Extension
{
    public static class GetTheFirstNCharacterOfTextExtension
    {
        /// <summary>
        /// get part of text 
        /// </summary>
        public static string GetTheFirstNCharacterOfText(this string s, int length)
        {
            if (String.IsNullOrEmpty(s))
                throw new ArgumentNullException(s);
            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Length > length)
                return words[0];
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                if ((sb + word).Length > length)
                    return string.Format("{0}...", sb.ToString().TrimEnd(' '));
                sb.Append(word + " ");
            }
            return string.Format("{0}...", sb.ToString().TrimEnd(' '));
        }
    }
}
