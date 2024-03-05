using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Caching;

namespace Webkit.Extensions.DataConversion
{
    public static class DataConversionExtensions
    {
        /// <summary>
        /// The options used when serializing data as Json
        /// </summary>
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        /// <summary>
        /// The options used when serializing data as XML
        /// </summary>
        public static XmlWriterSettings XmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
        };

        /// <summary>
        /// Gets the stream as a string.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string AsString(this Stream stream)
        {
            if (!stream.CanRead) throw new ArgumentException("This stream is not readable. Make sure Stream.CanRead is true.");

            using (StreamReader streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEndAsync().Result;
            }
        }

        /// <summary>
        /// Collapses the newlines in the string, making it one line.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Collapse(this string str)
        {
            return str.Replace(Environment.NewLine, "").Replace("\n", "");
        }

        /// <summary>
        /// Gets the header dictionary as a string.
        /// </summary>
        /// <param name="headerDictionary"></param>
        /// <returns></returns>
        public static string AsString(this IHeaderDictionary headerDictionary)
        {
            return String.Join(Environment.NewLine, headerDictionary.Select(header => $"{header.Key}: {String.Join("; ", header.Value)}"));
        }

        /// <summary>
        /// Converts the value to a json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="TypeAccessException"></exception>
        public static string AsJson<T>(this T value)
        {
            return JsonSerializer.Serialize(value, JsonSerializerOptions);
        }

        /// <summary>
        /// XmlSerializer uses assembly generation, and assemblies cannot be collected. It does some automatic cache/reuse for the simplest constructor scenarios (new XmlSerializer(Type), etc), but not for all scenarios, hence a custom cache implementation.
        /// </summary>
        static MemoryCache XmlSerializerCache = new MemoryCache("XmlSerializerCache");

        /// <summary>
        /// Converts the value to an XML sheet string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="TypeAccessException"></exception>
        public static string AsXml<T>(this T value)
        {
            string? qualifiedName = typeof(T).AssemblyQualifiedName;
            if (qualifiedName == null)
            {
                qualifiedName = typeof(T).FullName;
            }

            if (typeof(T).IsClass && typeof(T).IsNotPublic)
            {
                throw new TypeAccessException($"Cannot access '{typeof(T).FullName}' because it is not public");
            }

            XmlSerializer serializer;

            if (XmlSerializerCache.Contains(qualifiedName))
            {
                serializer = (XmlSerializer)XmlSerializerCache.Get(qualifiedName);

                // If the serializer has been accessed within the past 5 minutes, refresh the expiration.
                XmlSerializerCache.Set(qualifiedName, serializer, DateTime.Now.AddMinutes(5));
            }
            else
            {
                serializer = new XmlSerializer(typeof(T));
                XmlSerializerCache.Add(qualifiedName, serializer, DateTime.Now.AddMinutes(5));
            }

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, XmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, value);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
