using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Webkit.Security;

namespace Webkit.Sessions
{
    public class UserSessionCache
    {
        static MemoryCache Cache = new MemoryCache("sessions");

        /// <summary>
        /// Registers a token as a session
        /// </summary>
        /// <param name="token"></param>
        /// <returns>The session id of the token</returns>
        public static string Register(JsonSecurityToken token)
        {
            string sessionId = token.AsString();
            Cache.Add(sessionId, token, token.Expiration);

            return sessionId;
        }

        /// <summary>
        /// Returns the token of a session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static JsonSecurityToken Read(string sessionId)
        {
            return (JsonSecurityToken)Cache.Get(sessionId);
        }

        /// <summary>
        /// Returns a token if session is valid
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static JsonSecurityToken? ReadIfValid(string sessionId)
        {
            if(IsValid(sessionId))
            {
                return Read(sessionId);
            }

            return null;
        }
        
        /// <summary>
        /// Determines if a session is still valid
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static bool IsValid(string sessionId)
        {
            return Cache.Contains(sessionId);
        }
    }
}
