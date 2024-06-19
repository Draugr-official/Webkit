﻿using Microsoft.AspNetCore.Http;
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
using Newtonsoft.Json.Linq;
using Webkit.Security.TwoFactorAuthentication;
using Webkit.Email.SendGrid;
using SendGrid.Helpers.Mail;

namespace Webkit.Architectures.Default.Endpoints
{
    internal class RegisterEndpoint<T> where T : new()
    {
        static public IResult Register(HttpContext ctx, [FromBody] RegisterDto registerDto)
        {
            using DefaultArchitectureDatabaseContext? db = new T() as DefaultArchitectureDatabaseContext;

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
            if(!recipient.IsValid())
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

            if(DefaultArchitecturePack<T>.Config.RequireVerification)
            {

                VerificationCode verificationCode = new VerificationCode();
                user.VerificationCode = verificationCode;

                SendGridEmailClient emailClient = new SendGridEmailClient(DefaultArchitecturePack<T>.Config.SendGridApiKey, new EmailAddress(DefaultArchitecturePack<T>.Config.SenderEmailAddress, DefaultArchitecturePack<T>.Config.SenderEmailName));
                SendGrid.Response emailResponse = emailClient.Send(
                    recipient: recipient,
                    subject: $"Welcome to {DefaultArchitecturePack<T>.Config.ApplicationName}",
                    htmlBody: $"Your verification code is {verificationCode}",
                    rawBody: $"Your verification code is {verificationCode}"
                ).Result;

                if (!emailResponse.IsSuccessStatusCode)
                {
                    return Results.Problem("Could not send verification email");
                }
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
    }
}
