using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Webkit.Extensions.Logging;
using Webkit.Security;

namespace Webkit.Sessions
{
    public class UserSessionCache
    {
        /// <summary>
        /// Session duration in minutes
        /// </summary>
        public static int Duration = 60 * 2;

        static MemoryCache Cache = new MemoryCache("sessions");

        /// <summary>
        /// Registers a token as a session
        /// </summary>
        /// <param name="token"></param>
        /// <returns>The session id of the token</returns>
        public static string Register(JsonSecurityToken token, DateTime expiration)
        {
            string stringToken = token.AsBase64String();

            if(!IsValid(stringToken))
            {
                Cache.Add(stringToken, token, expiration);
            }

            return stringToken;
        }

        /// <summary>
        /// Returns the token of a session
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JsonSecurityToken Read(string token)
        {
            return (JsonSecurityToken)Cache.Get(token);
        }

        /// <summary>
        /// Refreshes the duration of a session
        /// </summary>
        /// <param name="token"></param>
        public static void RefreshDuration(string token)
        {
            JsonSecurityToken jsonSecurityToken = (JsonSecurityToken)Cache.Get(token);
            Cache.Set(token, jsonSecurityToken, DateTime.Now.AddMinutes(Duration));
        }

        /// <summary>
        /// Returns a token if session is valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JsonSecurityToken? ReadIfValid(string token)
        {
            if(IsValid(token))
            {
                return Read(token);
            }

            return null;
        }
        
        /// <summary>
        /// Determines if a session is still valid
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsValid(string token)
        {
            String.Join("\n", Cache.Select(t => t.Key + ": " + t.Value)).Log();

            return Cache.Contains(token);
        }
    }
}
