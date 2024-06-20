using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webkit.Architectures.Default.DTOs;
using Webkit.Models.EntityFramework;
using Webkit.Security.Password;
using Webkit.Security;
using Webkit.Sessions;
using Webkit.Attributes;
using Webkit.Extensions.DataConversion;

namespace Webkit.Architectures.Default.Endpoints
{
    [ApiController]
    internal class TelemetryEndpoint : ControllerBase
    {
        [Authenticate]
        [Authorize(Role = "Administrator")]
        static public IResult Telemetry()
        {
            return Results.Ok(TelemetryInsight.Diagnostics.AsJson());
        }
    }
}
