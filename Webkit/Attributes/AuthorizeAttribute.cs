using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.Logging;
using Webkit.Security;

namespace Webkit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        public string Role { get; set; } = "";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("token", out var token))
            {
                JsonSecurityToken? jsonSecurityToken = JsonSecurityToken.FromString(Encoding.UTF8.GetString(Convert.FromBase64String(token)));
                if (jsonSecurityToken == null)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                if (!jsonSecurityToken.Roles.Contains(Role))
                {
                    context.Result = new StatusCodeResult(403);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
