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
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Converts a byte array to a string using utf8
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string AsString(this byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        /// <summary>
        /// Converts a byte array to a string using specified encoding
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string AsString(this byte[] byteArray, Encoding encoding)
        {
            return encoding.GetString(byteArray);
        }

        /// <summary>
        /// Converts a string to a byte array using utf8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] AsByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Converts a string to a byte array using specified encoding
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] AsByteArray(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Joins an array into a string and returns it. Has no separator
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string AsString(this Array array)
        {
            return String.Join("", array);
        }

        /// <summary>
        /// Joins an array into a string with separators and returns it.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string AsString(this Array array, string separator)
        {
            return String.Join(separator, array);
        }

        /// <summary>
        /// Joins a list into a string and returns it. Has no separator
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string AsString<T>(this List<T> list)
        {
            return String.Join("", list);
        }

        /// <summary>
        /// Joins a list into a string with separators and returns it.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string AsString<T>(this List<T> list, string separator)
        {
            return String.Join(separator, list);
        }
    }
}
