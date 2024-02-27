using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Sessions;

namespace Webkit.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("login")]
        public LoginResponse Login([FromBody] LoginDto loginDto)
        {
            return new LoginResponse
            {
                SessionId = ""
            };
        }

        public class LoginDto
        {
            public string Username { get; set; } = "";

            public string Password { get; set; } = "";
        }

        public class LoginResponse
        {
            public string SessionId { get; set; } = "";
        }
    }
}
