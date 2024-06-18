﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Webkit.Architectures.Default.DTOs
{
    public class LoginDto
    {
        [DefaultValue("andbjorn")]
        [JsonPropertyName("username")]
        public string Username { get; set; } = "";

        [DefaultValue("123")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = "";
    }
}
