using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webkit.Attributes
{
    public class TelemetryAttribute : ActionFilterAttribute
    {
        // Add function to log to telemetry output if status code is bad
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
