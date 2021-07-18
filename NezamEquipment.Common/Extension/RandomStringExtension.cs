using System;
using System.Linq;

namespace NezamEquipment.Common.Extension
{
    public static class RandomStringExtension
    {
        public static string Get(int length, bool numberOfNonAlphanumericCharacters = false)
        {
            var random = new Random();

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefjhigklmnopqrstuvwxyz";

            if (numberOfNonAlphanumericCharacters)
            {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*(){}|?><";
            }

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
