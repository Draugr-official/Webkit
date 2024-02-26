
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Webkit.Mocking.EntityFramework;
using Webkit.Models.EntityFramework;
using Webkit.Security;

namespace Webkit.Test
{
    public class Program
    {
        class User : UserModel
        {
            public string Username { get; set; } = "";

            public string Email { get; set; } = "";
        }

        class Database : MockDbContext
        {
            public MockDbSet<User> Users { get; set; } = new MockDbSet<User>
            {
                new User
                    {
                        Username = "Test1",
                        Email = "Yoer@google.com",
                        Roles = new List<string>
                        {
                            "Administrator"
                        }
                    }
            };
        }

        public static void Main(string[] args)
        {
            using(Database db = new Database())
            {
                Console.WriteLine(new SecureToken().ToString());
            }
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
