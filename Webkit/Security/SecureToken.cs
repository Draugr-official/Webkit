using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Security
{
    /// <summary>
    /// A cryptographic secure token
    /// </summary>
    [Serializable]
    public class SecureToken
    {
        string Value = "";
        
        /// <summary>
        /// <inheritdoc cref="SecureToken"/>
        /// </summary>
        public SecureToken()
        {
            Value = NewToken();
        }

        /// <summary>
        /// Generates a cryptographic secure token
        /// </summary>
        /// <returns></returns>
        string NewToken()
        {
            return RandomNumberGenerator.GetString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 40);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
