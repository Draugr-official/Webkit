using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Webkit.Extensions;
using Webkit.Extensions.Console;
using Webkit.Extensions.DataConversion;
using Webkit.Extensions.Logging;

namespace Webkit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class TelemetryAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The logging method used in for Log extensions. Console.WriteLine is used by default.
        /// </summary>
        public static Action<TelemetryData> LogAction { get; set; } = (TelemetryData insights) =>
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
            string endpointKey = context.HttpContext.Request.Method + " " + context.HttpContext.Request.Path;

            TelemetryEndpointInsight endpointInsight;
            if (TelemetryInsight.Endpoints.ContainsKey(endpointKey))
            {
                endpointInsight = TelemetryInsight.Endpoints[endpointKey];
            }
            else
            {
                endpointInsight = new TelemetryEndpointInsight
                {
                    Method = context.HttpContext.Request.Method,
                    Path = context.HttpContext.Request.Path,

                    Success = 0,
                    Failure = 0,
                    Failures = new Dictionary<int, Dictionary<string, long>>()
                };

                TelemetryInsight.Endpoints.Add(endpointKey, endpointInsight);
            }
            
            if (context.HttpContext.Response.StatusCode < 400)
            {
                endpointInsight.Success += 1;
            }
            else
            {
                endpointInsight.Failure += 1;

                string failureKey = context.HttpContext.Response.Body.AsString();

                if (endpointInsight.Failures.ContainsKey(context.HttpContext.Response.StatusCode))
                {
                    if(endpointInsight.Failures[context.HttpContext.Response.StatusCode].ContainsKey(failureKey))
                    {
                        endpointInsight.Failures[context.HttpContext.Response.StatusCode][failureKey] += 1;
                    }
                    else
                    {
                        endpointInsight.Failures[context.HttpContext.Response.StatusCode].Add(failureKey, 1);
                    }
                }
                else
                {
                    endpointInsight.Failures.Add(context.HttpContext.Response.StatusCode, new Dictionary<string, long>
                    {
                        { failureKey, 1 }
                    });
                }
            }

            TelemetryInsight.Endpoints[endpointKey] = endpointInsight;

            LogAction(new TelemetryData
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

    /// <summary>
    /// The insights of every endpoint the TelemetryAttribute actionfilter has been assigned to
    /// </summary>
    public class TelemetryInsight
    {
        public static Dictionary<string, TelemetryEndpointInsight> Endpoints = new Dictionary<string, TelemetryEndpointInsight>();
    }

    public struct TelemetryEndpointInsight
    {
        /// <summary>
        /// The path of the endpoint
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The method used for the endpoint
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Amount of successful requests
        /// </summary>
        public long Success { get; set; }

        /// <summary>
        /// Amount of failed requests
        /// </summary>
        public long Failure { get; set; }

        /// <summary>
        /// A dictionary over failure messages and how many time they have failed
        /// </summary>
        public Dictionary<int, Dictionary<string, long>> Failures { get; set; }
    }

    public struct TelemetryData
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

        public TelemetryData()
        {
            Headers = new HeaderDictionary();
            HttpMethod = "";
            Path = "";
            Content = "";
        }
    }
}
