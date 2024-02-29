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
        /// <summary>
        /// The guid of the user
        /// </summary>
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// The roles assigned to the user
        /// </summary>
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        /// <summary>
        /// The date of when the token expires
        /// </summary>
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

        /// <summary>
        /// The universally unique signature of the token
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        
        /// <summary>
        /// <inheritdoc cref="SecureToken"/>
        /// </summary>
        public JsonSecurityToken(Guid userId, List<string> roles, DateTime expiration)
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
