using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webkit.Architectures.Default.DTOs;
using Webkit.Models.EntityFramework;
using Webkit.Security.Password;
using Webkit.Security;
using Webkit.Sessions;
using SendGrid.Helpers.Mail;
using Webkit.Email.SendGrid;
using Webkit.Security.TwoFactorAuthentication;
using Webkit.Attributes;
using System.Diagnostics;

namespace Webkit.Architectures.Default.Endpoints
{
    [ApiController]
    internal class Authentication : ControllerBase
    {
        /// <summary>
        /// Logs a user in and sets their Token cookie
        /// </summary>
        /// <typeparam name="T">Needs to be derived from <see cref="DefaultArchitectureDatabaseContext"/></typeparam>
        /// <param name="ctx"></param>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [Telemetry]
        public static IResult Login<T>(HttpContext ctx, [FromBody] LoginDto loginDto) where T : new()
        {
            T dbType = new T();
            if (dbType is not DefaultArchitectureDatabaseContext)
            {
                throw new Exception($"{typeof(T)} has to be derived from {nameof(DefaultArchitectureDatabaseContext)}!");
            }

            using DefaultArchitectureDatabaseContext? db = dbType as DefaultArchitectureDatabaseContext;
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

            if (user.SessionDuration > DateTime.Now)
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

            Activity.Current.SetStatus(ActivityStatusCode.Ok);

            return Results.Ok();

        }

        /// <summary>
        /// Registers a user and logs them in (by setting the Token cookie).
        /// <para>Sends them a verification email if the UseAccountVerification config is set to true.</para>
        /// </summary>
        /// <typeparam name="T">Needs to be derived from <see cref="DefaultArchitectureDatabaseContext"/></typeparam>
        /// <param name="ctx"></param>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [Telemetry]
        public static IResult Register<T>(HttpContext ctx, [FromBody] RegisterDto registerDto) where T : new()
        {
            T dbType = new T();
            if (dbType is not DefaultArchitectureDatabaseContext)
            {
                throw new Exception($"{typeof(T)} has to be derived from {nameof(DefaultArchitectureDatabaseContext)}!");
            }

            using DefaultArchitectureDatabaseContext? db = dbType as DefaultArchitectureDatabaseContext;
            if (db is null)
            {
                throw new Exception($"Db {typeof(T)} cannot be null!");
            }

            if (db.Users.Any(user => user.Username.Equals(registerDto.Username, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Conflict("This username is taken");
            }

            if (db.Users.Any(user => user.Email.Equals(registerDto.Email, StringComparison.OrdinalIgnoreCase) && user.IsVerified))
            {
                return Results.Conflict("This email is already in-use");
            }

            EmailAddress recipient = new EmailAddress(registerDto.Email);
            if (!recipient.IsValid())
            {
                return Results.BadRequest("Invalid email address");
            }

            UserModel user = new UserModel
            {
                Username = registerDto.Username,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Roles = new List<string>
                {
                    "User"
                }
            };

            if (DefaultArchitecturePack.Config.UseAccountVerification)
            {
                VerificationCode verificationCode = new VerificationCode();
                user.VerificationCode = verificationCode;

                SendGridEmailClient emailClient = new SendGridEmailClient(DefaultArchitecturePack.Config.SendGridApiKey, new EmailAddress(DefaultArchitecturePack.Config.SenderEmailAddress, DefaultArchitecturePack.Config.SenderEmailName));
                SendGrid.Response emailResponse = emailClient.Send(
                    recipient: recipient,
                    subject: $"Welcome to {DefaultArchitecturePack.Config.ApplicationName}",
                    htmlBody: $"Your verification code is {verificationCode}",
                    rawBody: $"Your verification code is {verificationCode}"
                ).Result;

                if (!emailResponse.IsSuccessStatusCode)
                {
                    return Results.BadRequest("Could not send verification email");
                }
            }
            else
            {
                user.IsVerified = true;
            }

            JsonSecurityToken jsonToken = new JsonSecurityToken(user.Id, user.Roles);
            DateTime tokenExpiration = DateTime.Now.AddMinutes(UserSessionCache.Duration);

            ctx.Response.Cookies.Append("token", jsonToken.AsBase64String(), new CookieOptions
            {
                Expires = tokenExpiration,
            });

            user.SessionToken = jsonToken.AsBase64String();
            user.SessionDuration = tokenExpiration;

            db.Users.Add(user);
            db.SaveChanges();

            return Results.Ok();
        }

        /// <summary>
        /// Takes in a code and checks that the user has verified their email
        /// </summary>
        /// <typeparam name="T">Needs to be derived from <see cref="DefaultArchitectureDatabaseContext"/></typeparam>
        /// <param name="ctx"></param>
        /// <param name="verifyDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Throws an exception if T is null</exception>
        [Telemetry]
        [Authenticate]
        public static IResult Verify<T>(HttpContext ctx, [FromBody] VerifyDto verifyDto) where T : new()
        {
            T dbType = new T();
            if (dbType is not DefaultArchitectureDatabaseContext)
            {
                throw new Exception($"{typeof(T)} has to be derived from {nameof(DefaultArchitectureDatabaseContext)}!");
            }

            using DefaultArchitectureDatabaseContext? db = dbType as DefaultArchitectureDatabaseContext;

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

            if (user.IsVerified)
            {
                return Results.Ok("Already verified");
            }

            if (user.VerificationCode != verifyDto.VerificationCode)
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
