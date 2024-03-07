using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Attributes;
using Webkit.Security;
using Webkit.Security.Password;
using Webkit.Sessions;

namespace Webkit.Test.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [Telemetry]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            using(MockDatabase mockDb = new MockDatabase())
            {
                List<User> users = mockDb.Users.Where(user => user.Username == loginDto.Username && PasswordHandler.Validate(loginDto.Password, user.Password)).ToList();
                if(!users.Any())
                {
                    return NotFound();
                }

                User user = users.First();

                JsonSecurityToken jsonToken = new JsonSecurityToken(user.Id, user.Roles);

                DateTime tokenExpiration = DateTime.Now.AddDays(30);
                string token = UserSessionCache.Register(jsonToken, tokenExpiration);
                user.Token = token;

                mockDb.SaveChanges();

                Response.Cookies.Append("token", token, new CookieOptions
                {
                    Expires = tokenExpiration,
                });
                return Ok();
            }
        }

        public class LoginDto
        {
            public string Username { get; set; } = "";

            public string Password { get; set; } = "";
        }
    }
}
