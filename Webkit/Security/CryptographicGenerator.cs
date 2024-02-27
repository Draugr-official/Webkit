using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Security
{
    public class CryptographicGenerator
    {
        /// <summary>
        /// Generates a cryptographically safe seed
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Seed(int length = 40)
        {
            return RandomNumberGenerator.GetString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 40);
        }

        /// <summary>
        /// Generates a cryptographically safe unicode seed
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string UnicodeSeed(int length = 40)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < length; i++)
            {
                stringBuilder.Append((char)RandomNumberGenerator.GetInt32(0x1000, 0x34ff));
            }

            return stringBuilder.ToString();
        }
    }
}
