using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.Logging;

namespace Webkit.Security.Password
{
    public class PasswordHandler
    {
        /// <summary>
        /// Hashes the password with a random salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Hash(string password)
        {
            string salt = CryptographicGenerator.UnicodeSeed(100);
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(String.Join("", SHA256.HashData(Encoding.Unicode.GetBytes(password + salt)).Select(b => b.ToString("X2"))) + ":" + salt));
        }

        /// <summary>
        /// Checks if the password matches the hash
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool Validate(string password, string hash)
        {
            try
            {
                hash = Encoding.Unicode.GetString(Convert.FromBase64String(hash));

                var parts = hash.Split(':');
                if (parts.Length != 2)
                {
                    throw new ArgumentException("Invalid format for hashed password.", nameof(hash));
                }

                var expectedHash = string.Join("", SHA256.HashData(Encoding.Unicode.GetBytes(password + parts[1])).Select(b => b.ToString("X2")));
                return expectedHash == parts[0];
            }
            catch { return false; }
        }
    }
}
