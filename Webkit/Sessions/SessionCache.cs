using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Webkit.Security;

namespace Webkit.Sessions
{
    public class SessionCache
    {
        static List<string> Sessions = new List<string>();

        public static void RegisterSession(JsonSecurityToken sessionToken)
        {
            MemoryCache.Default.Add(sessionToken.AsString(), sessionToken, sessionToken.Expiration);
        }
        
        public static bool IsSessionValid(string sessionId)
        {
            return MemoryCache.Default.Contains(sessionId);
        }
    }
}
