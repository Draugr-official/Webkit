using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Webkit.Architectures.Default.DTOs
{
    public class LoginDto
    {
        /// <summary>
        /// The username of the user
        /// </summary>
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; } = "";

        /// <summary>
        /// The password of the user
        /// </summary>
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }
}
