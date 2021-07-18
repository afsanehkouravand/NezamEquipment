using System.Security.Cryptography;
using System.Text;
using NezamEquipment.Common.Extension;

namespace NezamEquipment.Common.Security
{
    public static class Md5HashExtention
    {
        public static string Md5Hash()
        {
            var inputString = RandomStringExtension.Get(16);

            var md5 = MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(inputString);

            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            var hashedString = sb.ToString();

            return hashedString;
        }

        public static string Md5Hash(this string inputString)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(inputString);

            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            var hashedString = sb.ToString();

            return hashedString;
        }

        public static string Md5Hash(this string inputString, byte[] salt)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(inputString + salt);
            
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            var hashedString = sb.ToString();

            return hashedString;
        }

    }
}
