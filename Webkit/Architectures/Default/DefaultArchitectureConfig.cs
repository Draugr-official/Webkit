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

        /// <summary>
        /// Determines if the application will open the api/authentication/verify endpoint and send out verification codes on email to clients
        /// </summary>
        public bool RequireVerification { get; set; }

        /// <summary>
        /// The API key used for sending emails with sendgrid
        /// </summary>
        public string SendGridApiKey { get; set; } = "";

        /// <summary>
        /// The email address used in automated emails by the company
        /// </summary>
        public string SenderEmailAddress { get; set; } = "";

        /// <summary>
        /// The email name used in automated emails by the company
        /// </summary>
        public string SenderEmailName { get; set; } = "";

        /// <summary>
        /// The web application of the project
        /// </summary>
        public WebApplication WebApp { get; set; }
    }
}
