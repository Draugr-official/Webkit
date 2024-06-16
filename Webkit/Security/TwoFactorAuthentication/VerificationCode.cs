using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Security.TwoFactorAuthentication
{
    /// <summary>
    /// Creates a mew verification code. Generates using numbers and letters by default.
    /// </summary>
    public class VerificationCode
    {
        const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string Numbers = "0123456789";

        string Code { get; set; }

        public VerificationCode(int length = 6, VerificationCodeTypes type = VerificationCodeTypes.Letters | VerificationCodeTypes.Numbers)
        {
            string codeChoices = string.Empty;

            codeChoices += (type & VerificationCodeTypes.Letters) == VerificationCodeTypes.Letters ? Letters : "";
            codeChoices += (type & VerificationCodeTypes.Numbers) == VerificationCodeTypes.Numbers ? Numbers : "";

            Code = RandomNumberGenerator.GetString(codeChoices, length);
        }

        public static implicit operator string (VerificationCode verificationCode)
        {
            return verificationCode.Code;
        }
    }

    public enum VerificationCodeTypes
    {
        Letters,
        Numbers
    }
}
