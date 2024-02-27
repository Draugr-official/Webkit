using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Webkit.Security
{
    /// <summary>
    /// A cryptographic secure token
    /// </summary>
    [Serializable]
    public class JsonSecurityToken
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }
        
        /// <summary>
        /// <inheritdoc cref="SecureToken"/>
        /// </summary>
        public JsonSecurityToken(string userId, List<string> roles, DateTime expiration)
        {
            UserId = userId;
            Signature = CryptographicGenerator.Seed();
            Expiration = expiration;
            Roles = roles;
        }

        /// <summary>
        /// Converts the token to raw json and returns it
        /// </summary>
        /// <returns></returns>
        public string AsJson()
        {
            return JsonSerializer.Serialize(this);
        }
        
        /// <summary>
        /// Converts the token to a string and returns it
        /// </summary>
        /// <returns></returns>
        public string AsString()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this)));
        }

        /// <summary>
        /// Converts a string to a token
        /// </summary>
        /// <param name="rawJsonSecurityToken"></param>
        /// <returns></returns>
        public static JsonSecurityToken? FromString(string rawJsonSecurityToken)
        {
            return JsonSerializer.Deserialize<JsonSecurityToken>(rawJsonSecurityToken);
        }
        
        public override string ToString()
        {
            return AsString();
        }
    }
}
