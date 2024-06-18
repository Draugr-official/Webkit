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
        public string Id { get; set; } = "";

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string Email { get; set; } = "";

        public List<Guid> Channels { get; set; } = new List<Guid>();
    }

    public class Channel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public ChannelType Type { get; set; }
    }

    public enum ChannelType
    {
        DirectMessage,
        Group
    }

    public class Message
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid ChannelId { get; set; }
    }

    public class ChannelBinding
    {
        public Guid UserId { get; set; }

        public Guid ChannelId { get; set; }
    }

    public class MockDatabase : DefaultArchitectureDatabaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Proddb");
        }
    }
}
