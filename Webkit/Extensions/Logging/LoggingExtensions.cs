using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Webkit.Extensions.DataConversion;

namespace Webkit.Extensions.Logging
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// The logging method used in for Log extensions. Console.WriteLine is used by default.
        /// </summary>
        public static Action<object> DefaultLog { get; set; } = System.Console.WriteLine;

        /// <summary>
        /// Writes the value to the default output. If T is not a primitive type, value will be serialized as json.
        /// </summary>
        public static void LogAsXml<T>(this T value)
        {
            LogAsXml(value, "", "");
        }

        /// <summary>
        /// <inheritdoc cref="Log{T}(T)"/>
        /// </summary>
        public static void LogAsXml<T>(this T value, string prependText)
        {
            LogAsXml(value, prependText, "");
        }

        /// <summary>
        /// <inheritdoc cref="Log{T}(T)"/>
        /// </summary>
        public static void LogAsXml<T>(this T value, string prependText, string appendText)
        {
            Log(value.AsXml(), prependText, appendText);
        }

        /// <summary>
        /// Writes the value to the default output. If T is not a primitive type, value will be serialized as json.
        /// </summary>
        public static void LogAsJson<T>(this T value)
        {
            LogAsJson(value, "", "");
        }

        /// <summary>
        /// <inheritdoc cref="LogAsJson{T}(T)"/>
        /// Prepends text to the beginning of the value.
        /// </summary>
        public static void LogAsJson<T>(this T value, string prependText)
        {
            LogAsJson(value, prependText, "");
        }

        /// <summary>
        /// <inheritdoc cref="LogAsJson{T}(T)"/>
        /// Prepends text to the beginning and appends text to the ending of the value.
        /// </summary>
        public static void LogAsJson<T>(this T value, string prependText, string appendText)
        {
            Log(prependText + value.AsJson() + appendText);
        }

        /// <summary>
        /// Writes the value to the default output.
        /// </summary>
        public static void Log<T>(this T value)
        {
            if (value == null)
            {
                DefaultLog("null");
                return;
            }

            DefaultLog(value);
        }

        /// <summary>
        /// <inheritdoc cref="Log{T}(T)"/>
        /// Prepends text to the beginning of the value.
        /// </summary>
        public static void Log<T>(this T value, string prependText)
        {
            Log(prependText + value);
        }

        /// <summary>
        /// <inheritdoc cref="Log{T}(T)"/>
        /// Prepends text to the beginning and appends text to the ending of the value.
        /// </summary>
        public static void Log<T>(this T value, string prependText, string appendText)
        {
            Log(prependText + value + appendText);
        }
    }
}
