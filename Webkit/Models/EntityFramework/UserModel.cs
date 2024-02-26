using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webkit.Mocking.EntityFramework;

namespace Webkit.Models.EntityFramework
{
    /// <summary>
    /// Contains certain properties required for Webkit to work properly
    /// </summary>
    public class UserModel
    {
        public List<string> Roles { get; set; } = new List<string>();

        public string Token { get; set; } = "";
    }
}
