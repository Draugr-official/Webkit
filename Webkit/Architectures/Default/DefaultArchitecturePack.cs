using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SendGrid;
using Webkit.Security.Password;
using Webkit.Security;
using Webkit.Sessions;
using Webkit.Models.EntityFramework;
using Webkit.Architectures.Default.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Webkit.Extensions.Logging;
using Webkit.Attributes;
using Webkit.Architectures.Default.Endpoints;

namespace Webkit.Architectures.Default
{
    /// <summary>
    /// An architecture pack used to automatically set up the application for a certain application architecture style
    /// <para>Registers 3 new endpoints.</para>
    /// <list type="bullet">
    ///     <item>api/authentication/login</item>
    ///     <item>api/authentication/register</item>
    ///     <item>api/authentication/verify</item>
    /// </list>
    /// </summary>
    public class DefaultArchitecturePack
    {
        public static DefaultArchitectureConfig Config { get; set; } = new DefaultArchitectureConfig();

        /// <summary>
        /// Loads the pack into the application
        /// </summary>
        /// <typeparam name="T">The DbContext of your application - MUST be derived from DefaultArchitectureDatabaseContext</typeparam>
        /// <param name="config"></param>
        /// <exception cref="Exception"></exception>
        public static void Load<T>(DefaultArchitectureConfig config) where T : new()
        {
            Config = config;

            config.WebApp.MapPost("api/authentication/login", Authentication.Login<T>);
            config.WebApp.MapPost("api/authentication/register", Authentication.Register<T>);
            
            if(config.UseAccountVerification)
            {
                config.WebApp.MapPost("api/authentication/verify", Authentication.Verify<T>);
            }

            if(config.UseTelemetry)
            {
                config.WebApp.MapGet("api/telemetry", TelemetryEndpoint.Telemetry);
            }

            AuthenticateAttribute.Validate = (string token) =>
            {
                using (DefaultArchitectureDatabaseContext? db = new T() as DefaultArchitectureDatabaseContext)
                {
                    if (db is null)
                    {
                        throw new Exception($"Db {typeof(T)} cannot be null!");
                    }

                    if(db.Users.Any(user => user.SessionToken == token && user.SessionDuration > DateTime.Now))
                    {
                        return true;
                    }
                }

                return false;
            };
        }
    }
}
