using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Webkit.Extensions
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// The logging method used in for Log extensions. Console.WriteLine is used by default.
        /// </summary>
        public static Action<object> DefaultLog { get; set; } = Console.WriteLine;

        /// <summary>
        /// The options used when serializing data
        /// </summary>
        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        /// <summary>
        /// XmlSerializer uses assembly generation, and assemblies cannot be collected. It does some automatic cache/reuse for the simplest constructor scenarios (new XmlSerializer(Type), etc), but not for all scenarios, hence a custom cache implementation.
        /// </summary>
        static MemoryCache XmlSerializerCache = new MemoryCache("XmlSerializerCache");

        /// <summary>
        /// Writes the value to the default output. If T is not a primitive type, value will be serialized as json.
        /// </summary>
        public static void LogAsXml<T>(this T value)
        {
            string? qualifiedName = typeof(T).AssemblyQualifiedName;
            if(qualifiedName == null)
            {
                qualifiedName = typeof(T).FullName;
            }

            if(typeof(T).IsNotPublic)
            {
                Log(qualifiedName, "Cannot serialize a non-public object to XML");
                return;
            }

            XmlSerializer serializer;

            if (XmlSerializerCache.Contains(qualifiedName))
            {
                serializer = (XmlSerializer)XmlSerializerCache.Get(qualifiedName);

                // If the serializer has been accessed within the past 5 minutes, refresh the expiration.
                XmlSerializerCache.Set(qualifiedName, serializer, DateTime.Now.AddMinutes(5));

                Log("");
            }
            else
            {
                serializer = new XmlSerializer(typeof(T));
                XmlSerializerCache.Add(qualifiedName, serializer, DateTime.Now.AddMinutes(5));
            }

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, value);
                    Log(stringWriter.ToString());
                }
            }
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
            if(typeof(T).IsSealed)
            {
                Log("Grrr illegale!");
                return;
            }

            Log(prependText + JsonSerializer.Serialize(value, SerializerOptions) + appendText);
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
