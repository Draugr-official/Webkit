using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Security;
using Webkit.Security.Password;
using Webkit.Sessions;

namespace Webkit.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginDto loginDto)
        {
            using(MockDatabase mockDb = new MockDatabase())
            {
                List<User> users = mockDb.Users.Where(user => user.Username == loginDto.Username && PasswordHandler.Validate(loginDto.Password, user.Password)).ToList();
                if(!users.Any())
                {
                    return NotFound();
                }

                User user = users.First();

                string sessionId = UserSessionCache.Register(new JsonSecurityToken(user.Id, user.Roles, DateTime.Now.AddDays(30)));
                user.Token = sessionId;

                mockDb.SaveChanges();

                return new LoginResponse
                {
                    SessionId = sessionId
                };
            }
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
