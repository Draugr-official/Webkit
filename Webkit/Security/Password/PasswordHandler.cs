using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.Logging;
using Konscious.Security.Cryptography;

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
            using(Argon2id argon = new Argon2id(Encoding.Unicode.GetBytes(password + salt))
            {
                Iterations = 2,
                DegreeOfParallelism = 1,
                MemorySize = 19923
            })
            {
                return Convert.ToBase64String(Encoding.Unicode.GetBytes(String.Join("", argon.GetBytes(1024).Select(b => b.ToString("X2"))) + ":" + salt));
            }
        }

        /// <summary>
        /// Checks if the password matches the hash
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static bool Validate(string password, string hashedPassword)
        {
            try
            {
                hashedPassword = Encoding.Unicode.GetString(Convert.FromBase64String(hashedPassword));

                var parts = hashedPassword.Split(':');
                if (parts.Length != 2)
                {
                    throw new ArgumentException("Invalid format for hashed password.", nameof(hashedPassword));
                }

                using(Argon2id argon = new Argon2id(Encoding.Unicode.GetBytes(password + parts[1]))
                {
                    Iterations = 2,
                    DegreeOfParallelism = 1,
                    MemorySize = 19923
                })
                {
                    var expectedHash = string.Join("", argon.GetBytes(1024).Select(b => b.ToString("X2")));
                    return expectedHash == parts[0];
                }
            }
            catch { return false; }
        }
    }
}
