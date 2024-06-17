using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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
        [Key]
        public long Id { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Username { get; set; } = "";

        [PasswordPropertyText]
        public string Password { get; set; } = "";

        [EmailAddress]
        public string Email { get; set; } = "";

        public List<string> Roles { get; set; } = new List<string>();

        internal string Token { get; set; } = "";
    }
}
