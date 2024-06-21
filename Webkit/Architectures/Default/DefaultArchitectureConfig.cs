using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Webkit.Architectures.Default
{
    public class DefaultArchitectureConfig
    {
        /// <summary>
        /// This will be used in, for example, welcome emails to users.
        /// </summary>
        public string ApplicationName { get; set; } = "";

        /// <summary>
        /// Determines if the application will open the 'api/authentication/verify' endpoint and send out verification codes on email to clients
        /// </summary>
        public bool UseAccountVerification { get; set; }

        /// <summary>
        /// Determines if the application will open the 'api/telemetry' endpoint and log all responses to create a diagnostics schematic
        /// </summary>
        public bool UseTelemetry { get; set; }

        /// <summary>
        /// Determines the role name required to access sensitive information such as telemetry data set up by <see cref="DefaultArchitecturePack{T}"/>
        /// <para>Default is 'Administrator'.</para>
        /// </summary>
        public string AdministratorRoleName { get; set; } = "Administrator";

        /// <summary>
        /// Determines how long a user session should last after logging in before the session expires (in minutes)
        /// <para>Defaulted to 43 200 minutes (30 days)</para>
        /// </summary>
        public int UserSessionLength { get; set; } = 60 * 24 * 30;

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
