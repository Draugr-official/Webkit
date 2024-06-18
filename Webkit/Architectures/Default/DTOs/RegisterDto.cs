using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Webkit.Architectures.Default.DTOs
{
    internal class RegisterDto
    {
        [DefaultValue("Andy")]
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = "";

        [DefaultValue("Jacobsen")]
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = "";

        [DefaultValue("andbjorn")]
        [JsonPropertyName("username")]
        public string Username { get; set; } = "";

        [DefaultValue("andbjornwil@gmail.com")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [DefaultValue("123")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }
}
