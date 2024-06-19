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

                List<UserModel> users = db.Users.Where(user => user.SessionToken == ctx.Request.Cookies["token"] && user.SessionDuration > DateTime.Now).ToList();
                if (!users.Any())
                {
                    return Results.Unauthorized();
                }

                UserModel user = users.First();

                if(user.IsVerified)
                {
                    return Results.Ok("Already verified");
                }

                if(user.VerificationCode != verifyDto.VerificationCode)
                {
                    return Results.BadRequest("Invalid verification code");
                }

                user.IsVerified = true;
                user.VerificationCode = "";

                db.SaveChanges();

                return Results.Ok();
            }
        }
    }
}
