using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Attributes;
using Webkit.Extensions.DataConversion;
using Webkit.Extensions.Logging;

namespace Webkit.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Telemetry]
        [HttpGet]
        public ActionResult Test()
        {
            using (MockDatabase db = new MockDatabase())
            {
                return Ok(db.Users.AsJson());
            }
        }
    }
}
