using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Webkit.Security;

namespace Webkit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        [Required]
        public string Role { get; set; } = "";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationContent))
            {
                JsonSecurityToken? jsonSecurityToken = JsonSecurityToken.FromString(authorizationContent);
                if(jsonSecurityToken == null)
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                if (!jsonSecurityToken.Roles.Contains(Role))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }
    }
}
