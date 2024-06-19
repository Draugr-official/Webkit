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
using Webkit.Extensions.Logging;
using Webkit.Security;
using Webkit.Sessions;

namespace Webkit.Attributes
{
    /// <summary>
    /// Reads the Authorization header and determines if the JsonSecurityToken is valid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthenticateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The default method for checking if a token is still valid. Can be assigned with own implementation.
        /// </summary>
        public static Func<string, bool> Validate = UserSessionCache.IsValid;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("token", out string token))
            {
                token.Log("Token: ");

                JsonSecurityToken? jsonSecurityToken = JsonSecurityToken.FromString(Encoding.UTF8.GetString(Convert.FromBase64String(token)));
                if (jsonSecurityToken == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // If token is valid
                if (!Validate(token))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                return;
            }

            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
