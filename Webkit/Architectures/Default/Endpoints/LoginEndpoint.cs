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

namespace Webkit.Architectures.Default.Endpoints
{
    internal class LoginEndpoint<T> where T : new()
    {
        static public IResult Login(HttpContext ctx, [FromBody] LoginDto loginDto)
        {
            using (DefaultArchitectureDatabaseContext? db = new T() as DefaultArchitectureDatabaseContext)
            {
                if (db is null)
                {
                    throw new Exception($"Db {typeof(T)} cannot be null!");
                }

                List<UserModel> users = db.Users.Where(user => user.Username == loginDto.Username && PasswordManager.Verify(loginDto.Password, user.Password)).ToList();
                if (!users.Any())
                {
                    return Results.NotFound();
                }

                UserModel user = users.First();

                if(user.SessionDuration > DateTime.Now)
                {
                    ctx.Response.Cookies.Append("token", user.SessionToken, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(UserSessionCache.Duration)
                    });

                    return Results.Ok();
                }

                JsonSecurityToken jsonToken = new JsonSecurityToken(user.Id, user.Roles);
                DateTime tokenExpiration = DateTime.Now.AddMinutes(UserSessionCache.Duration);

                user.SessionToken = jsonToken.AsBase64String();
                user.SessionDuration = tokenExpiration;

                db.SaveChanges();

                ctx.Response.Cookies.Append("token", jsonToken.AsBase64String(), new CookieOptions
                {
                    Expires = tokenExpiration,
                });

                return Results.Ok();
            }
        }
    }
}
