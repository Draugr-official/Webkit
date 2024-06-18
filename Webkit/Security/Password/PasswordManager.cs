using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.Logging;
using BCrypt.Net;
using Webkit.Extensions.DataConversion;

namespace Webkit.Security.Password
{
    public class PasswordManager
    {
        /// <summary>
        /// Hashes the password with a random salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 10);
        }

        /// <summary>
        /// Checks if the password matches the hash
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
