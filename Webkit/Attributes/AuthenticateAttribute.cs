using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Webkit.Security;

namespace Webkit.Attributes
{
    /// <summary>
    /// Reads the Authorization header and determines if the token is valid
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (ValidateToken == null)
            {
                throw new NotImplementedException("You must set the AuthenticateAttribute.ValidateToken delegate to use the Authenticate attribute.");
            }

            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues Authorization))
            {
                string authorizationContent = Authorization.First();

                Span<byte> base64Bytes = new Span<byte>();
                if (Convert.TryFromBase64String(authorizationContent, base64Bytes, out int _))
                {
                    string rawJsonSecurityToken = Encoding.UTF8.GetString(base64Bytes);
                    JsonSecurityToken? jsonSecurityToken = JsonSecurityToken.FromString(rawJsonSecurityToken);
                    if (jsonSecurityToken == null)
                    {
                        context.Result = new BadRequestResult();
                        return;
                    }

                    // If token has expired
                    if (jsonSecurityToken.Expiration < DateTime.Now)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    // If token is valid
                    if (!ValidateToken(rawJsonSecurityToken))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
            }
            else
            {
                context.Result = new BadRequestResult();
                return;
            }

            // success
        }
        
        public static Func<string, bool>? ValidateToken { get; set; }
    }
}
