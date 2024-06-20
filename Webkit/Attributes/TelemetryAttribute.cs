using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Specialized;
using System.Runtime.Caching;
using Webkit.Architectures.Default;
using Webkit.Extensions;
using Webkit.Extensions.Console;
using Webkit.Extensions.DataConversion;
using Webkit.Extensions.Logging;

namespace Webkit.Attributes
{
    /// <summary>
    /// An attribute to make diagnostic reports and see how endpoints are performing
    /// <para>Access <see cref="TelemetryInsight.Diagnostics"/> to read endpoint diagnostic</para>
    /// <para>Assign the <see cref="Log"/> action to set your own logger</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class TelemetryAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The logging method used in for Log extensions. Console.WriteLine is used by default.
        /// </summary>
        public static Action<TelemetryData> Log { get; set; } = (TelemetryData data) =>
        {
            TelemetryEndpointInsight endpointInsight;
            if (TelemetryInsight.Diagnostics.ContainsKey(data.Path))
            {
                endpointInsight = TelemetryInsight.Diagnostics[data.Path][data.HttpMethod];
            }
            else
            {
                endpointInsight = new TelemetryEndpointInsight
                {
                    Good = 0,
                    Bad = 0,
                    Failures = new Dictionary<int, Dictionary<string, long>>()
                };

                TelemetryInsight.Diagnostics.Add(data.Path, new Dictionary<string, TelemetryEndpointInsight>
                {
                    { data.HttpMethod, endpointInsight }
                });
            }

            if (data.IsSuccess)
            {
                endpointInsight.Good += 1;
            }
            else
            {
                endpointInsight.Bad += 1;

                if (endpointInsight.Failures.ContainsKey(data.HttpStatusCode))
                {
                    if (endpointInsight.Failures[data.HttpStatusCode].ContainsKey(data.Content))
                    {
                        endpointInsight.Failures[data.HttpStatusCode][data.Content] += 1;
                    }
                    else
                    {
                        endpointInsight.Failures[data.HttpStatusCode].Add(data.Content, 1);
                    }
                }
                else
                {
                    endpointInsight.Failures.Add(data.HttpStatusCode, new Dictionary<string, long>
                    {
                        { data.Content, 1 }
                    });
                }
            }

            TelemetryInsight.Diagnostics[data.Path][data.HttpMethod] = endpointInsight;
        };

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if(DefaultArchitecturePack.Config.UseTelemetry)
            {
                Log(new TelemetryData
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
    }

    /// <summary>
    /// The insights of every endpoint the TelemetryAttribute actionfilter has been assigned to
    /// </summary>
    public class TelemetryInsight
    {
        /// <summary>
        /// The telemetry data of each endpoint (if there has been any data for given endpoint)
        /// <para>Structure: TelemetryEndpointInsight TelemetryInsight.Diagnostics[path][method]</para>
        /// </summary>
        public static Dictionary<string, Dictionary<string, TelemetryEndpointInsight>> Diagnostics = new Dictionary<string, Dictionary<string, TelemetryEndpointInsight>>();
    }

    public struct TelemetryEndpointInsight
    {
        /// <summary>
        /// Amount of successful requests
        /// </summary>
        public long Good { get; set; }

        /// <summary>
        /// Amount of failed requests
        /// </summary>
        public long Bad { get; set; }

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
