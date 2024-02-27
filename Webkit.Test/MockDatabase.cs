using Webkit.Mocking.EntityFramework;
using Webkit.Models.EntityFramework;

namespace Webkit.Test
{

    class User : UserModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string Email { get; set; } = "";

        public List<Guid> Channels { get; set; } = new List<Guid>();
    }

    class Channel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public ChannelType Type { get; set; }
    }

    enum ChannelType
    {
        DirectMessage,
        Group
    }

    class Message
    {
        public Guid Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid ChannelId { get; set; }
    }

    class ChannelBinding
    {
        public Guid UserId { get; set; }

        public Guid ChannelId { get; set; }
    }

    class MockDatabase : MockDbContext
    {
        public static Guid ChannelId = Guid.NewGuid();

        public static Guid Test1UserId = Guid.NewGuid();
        public static Guid RashUserId = Guid.NewGuid();

        public MockDbSet<User> Users { get; set; } = new MockDbSet<User>
            {
                new User
                    {
                        Id = Test1UserId,
                        Username = "Test1",
                        Email = "Yoer@google.com",
                        Roles = new List<string>
                        {
                            "Administrator"
                        },
                        Channels = new List<Guid>
                        {
                            ChannelId
                        },
                    },
                new User
                    {
                        Id = RashUserId,
                        Username = "Rash",
                        Email = "Balter@google.com",
                        Roles = new List<string>
                        {
                            "User"
                        },
                        Channels = new List<Guid>
                        {
                            ChannelId
                        },
                    },
            };

        public MockDbSet<Channel> Channels { get; set; } = new MockDbSet<Channel>
            {
                new Channel
                {
                    Id = ChannelId,
                    Name = "E-commerce project",
                    Type = ChannelType.Group,
                }
            };

        public MockDbSet<ChannelBinding> Bindings { get; set; } = new MockDbSet<ChannelBinding>
            {
                new ChannelBinding
                {
                    ChannelId = ChannelId,
                    UserId = Test1UserId,
                },
                new ChannelBinding
                {
                    ChannelId = ChannelId,
                    UserId = RashUserId,
                },
            };
    }
}
