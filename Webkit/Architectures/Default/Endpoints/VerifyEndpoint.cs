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
    internal class VerifyEndpoint<T> where T : new()
    {
        static public IResult Verify(HttpContext ctx, [FromBody] VerifyDto verifyDto)
        {
            using (DefaultArchitectureDatabaseContext? db = new T() as DefaultArchitectureDatabaseContext)
            {
                if (db is null)
                {
                    throw new Exception($"Db {typeof(T)} cannot be null!");
                }

                List<UserModel> users = db.Users.Where(user => user.Username == verifyDto.Username && PasswordManager.Verify(verifyDto.Password, user.Password)).ToList();
                if (!users.Any())
                {
                    return Results.NotFound();
                }

                UserModel user = users.First();

                if (UserSessionCache.IsValid(user.Token))
                {
                    UserSessionCache.RefreshDuration(user.Token);

                    ctx.Response.Cookies.Append("token", user.Token, new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(UserSessionCache.Duration),
                    });
                    return Results.Ok();
                }

                // If a session does not already exist, create a new one.
                JsonSecurityToken jsonToken = new JsonSecurityToken(user.Id, user.Roles);

                DateTime tokenExpiration = DateTime.Now.AddMinutes(UserSessionCache.Duration);
                string token = UserSessionCache.Register(jsonToken, tokenExpiration);
                user.Token = token;

                db.SaveChanges();

                ctx.Response.Cookies.Append("token", token, new CookieOptions
                {
                    Expires = tokenExpiration,
                });
                return Results.Ok();
            }
        }
    }
}
