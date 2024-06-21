using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webkit.Models.EntityFramework;

namespace Webkit.Architectures.Default
{
    public class DefaultArchitectureDatabaseContext : DbContext
    {
        public virtual DbSet<UserModel> Users { get; set; }
    }

    public static class DefaultArchitectureDatabaseContextExtensions
    {
        /// <summary>
        /// Performs a migration on the database.</br>
        /// Never use before the architecture pack has been loaded.
        /// </summary>
        public static void Migrate(this DefaultArchitectureDatabaseContext ctx)
        {
            ctx.Database.Migrate();
        }
    }
}
