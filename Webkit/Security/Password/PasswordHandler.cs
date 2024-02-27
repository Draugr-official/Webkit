using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static bool Validate(string password, string hashedPassword)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentNullException(nameof(hashedPassword), "Hashed password cannot be null or empty.");
            }

            // Split the hashed password and salt
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid format for hashed password.", nameof(hashedPassword));
            }

            var salt = Convert.FromBase64String(parts[1]);

            // Generate the expected hash with the provided password and salt
            var expectedHash = Convert.ToBase64String(Encoding.Unicode.GetBytes(
                string.Join("", SHA256.HashData(Encoding.Unicode.GetBytes(password + salt)))
                + ":" + Convert.ToBase64String(salt)));

            // Compare the generated hash with the provided hashed password
            return expectedHash == hashedPassword;
        }

        // TODO: Add password checker
    }
}
