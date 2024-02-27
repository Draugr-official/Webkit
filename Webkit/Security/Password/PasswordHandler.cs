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
        public static string Secure(string password)
        {
            string salt = CryptographicGenerator.UnicodeSeed(100);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Join("", SHA256.HashData(Encoding.UTF8.GetBytes(password + salt)).Select(b => b.ToString("X2"))) + ":" + salt));
        }

        // TODO: Add password checker
    }
}
