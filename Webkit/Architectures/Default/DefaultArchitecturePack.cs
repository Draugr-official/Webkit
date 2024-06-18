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
    /// <typeparam name="T">The DbContext of your application - MUST be derived from DefaultArchitectureDatabaseContext</typeparam>
    public class DefaultArchitecturePack<T> where T : new()
    {
        public static DefaultArchitectureConfig Config { get; set; } = new DefaultArchitectureConfig();

        public static void Load(DefaultArchitectureConfig config)
        {
            Config = config;

            config.WebApp.MapPost("api/authentication/login", LoginEndpoint<T>.Login);
            config.WebApp.MapPost("api/authentication/register", RegisterEndpoint<T>.Register);
            config.WebApp.MapPost("api/authentication/verify", VerifyEndpoint<T>.Verify);
        }
    }
}
