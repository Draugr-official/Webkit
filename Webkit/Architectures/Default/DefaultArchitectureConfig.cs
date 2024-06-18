using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Architectures.Default
{
    public class DefaultArchitectureConfig
    {
        /// <summary>
        /// This will be used in, for example, welcome emails to users.
        /// </summary>
        public string ApplicationName { get; set; } = "";

        public string SendGridApiKey { get; set; } = "";

        public string SenderEmailAddress { get; set; } = "";

        public string SenderEmailName { get; set; } = "";

        public WebApplication WebApp { get; set; } = "";
    }
}
