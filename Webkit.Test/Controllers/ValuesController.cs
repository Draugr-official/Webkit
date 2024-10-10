using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Attributes;

namespace Webkit.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Telemetry]
        [Authenticate]
        [Authorize(Role = "Administrator")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("This endpoint was called successfully!");
        }
    }
}
