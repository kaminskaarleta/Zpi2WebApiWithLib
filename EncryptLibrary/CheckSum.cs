using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptLibrary
{
    public class CheckSum
    {
        public static string CalculateMD5(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return ToStringRepresentation(md5.ComputeHash(stream));
                }
            }
        }

        public static string CalculateSHA1(string fileName)
        {
            using (var sha1 = SHA1.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return ToStringRepresentation(sha1.ComputeHash(stream));
                }
            }
        }

        private static string ToStringRepresentation(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
        }
    }
}
