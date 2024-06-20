using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Webkit.Architectures.Default;
using Webkit.Mocking.EntityFramework;
using Webkit.Models.EntityFramework;
using Webkit.Security.Password;

namespace Webkit.Test
{

    public class User : UserModel
    {
        public List<Guid> Channels { get; set; } = new List<Guid>();
    }


    public class MockDatabase : DefaultArchitectureDatabaseContext
    {
        public new DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Proddb");
        }
    }
}
