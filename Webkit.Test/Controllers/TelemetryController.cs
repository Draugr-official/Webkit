using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Attributes;
using Webkit.Extensions.DataConversion;

namespace Webkit.Test.Controllers
{
    [Route("api/telemetry")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        [Authenticate]
        [Authorize(Role = "Administrator")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(TelemetryInsight.Endpoints.AsJson());
        }
    }
}
