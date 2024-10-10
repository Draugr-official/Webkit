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
    internal class VerifyDto
    {
        [Required]
        [JsonPropertyName("verificationCode")]
        public string VerificationCode { get; set; } = "";
    }
}
