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

namespace Webkit.Architectures.Default
{
    public class DefaultArchitecturePack
    {
        public static void Load(WebApplication app, string sqlConnectionString)
        {
            app.MapPost("api/authentication/login", Login);
        }

        static public ActionResult Login(HttpContext ctx, [FromBody] LoginDto loginDto)
        {
            using (DefaultArchitectureDatabaseContext db = new DefaultArchitectureDatabaseContext())
            {
                List<UserModel> users = db.Users.Where(user => user.Username == loginDto.Username && PasswordHandler.Verify(loginDto.Password, user.Password)).ToList();
                if (!users.Any())
                {
                    return new NotFoundResult();
                }

                UserModel user = users.First();
                JsonSecurityToken jsonToken = new JsonSecurityToken(user.Id, user.Roles);

                DateTime tokenExpiration = DateTime.Now.AddDays(30);
                string token = UserSessionCache.Register(jsonToken, tokenExpiration);
                user.Token = token;

                db.SaveChanges();

                ctx.Response.Cookies.Append("token", token, new CookieOptions
                {
                    Expires = tokenExpiration,
                });
                return new OkResult();
            }
        }
    }
}
