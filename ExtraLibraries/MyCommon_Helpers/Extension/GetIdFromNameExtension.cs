using System;
using System.Text.RegularExpressions;

namespace MyCommon.Helpers.Extension
{
    internal static class GetIdFromNameExtension
    {
        private static readonly Regex InvalidChars = new Regex(@"[^A-Z0-9]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        internal static string GetIdFromName(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return InvalidChars.Replace(input, "_");
            }

            return input;
        }
    }
}