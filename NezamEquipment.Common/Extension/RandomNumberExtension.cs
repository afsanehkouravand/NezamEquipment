using System;
using System.Linq;

namespace NezamEquipment.Common.Extension
{
    public static class RandomNumberExtension
    {
        public static string Get(int length, int? startWith = null)
        {
            var random = new Random();

            var number = new string(Enumerable.Repeat("1234567890", length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return startWith != null ? startWith.Value + number : number;
        }
    }
}
