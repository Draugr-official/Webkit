using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Webkit.Extensions;
using Webkit.Extensions.Console;
using Webkit.Extensions.DataConversion;

namespace Webkit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class TelemetryAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The logging method used in for Log extensions. Console.WriteLine is used by default.
        /// </summary>
        public static Action<TelemetryInsights> LogAction { get; set; } = (TelemetryInsights insights) =>
        {
            if (insights.IsSuccess)
            {
                ConsoleColor.White.WriteLine(insights.AsJson());
                return;
            }

            ConsoleColor.Red.WriteLine(insights.AsJson());
        };

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            LogAction(new TelemetryInsights
            {
                IsSuccess = context.HttpContext.Response.StatusCode < 400,
                HttpStatusCode = context.HttpContext.Response.StatusCode,
                Timestamp = DateTime.Now,
                Headers = context.HttpContext.Request.Headers,
                HttpMethod = context.HttpContext.Request.Method,
                Path = context.HttpContext.Request.Path,
                Content = context.HttpContext.Request.Body.CanRead ? context.HttpContext.Request.Body.AsString() : ""
            });
        }
    }

    public struct TelemetryInsights
    {
        /// <summary>
        /// Determines if the response has a successful status code
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The http status code of the response
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// When the request happened
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The headers of the request
        /// </summary>
        public IHeaderDictionary Headers { get; set; }

        /// <summary>
        /// The http method of the request
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// The path of the request
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The content of the request
        /// </summary>
        public string Content { get; set; }

        public TelemetryInsights()
        {
            Headers = new HeaderDictionary();
            HttpMethod = "";
            Path = "";
            Content = "";
        }
    }
}
