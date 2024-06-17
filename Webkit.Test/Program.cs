
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Webkit.Attributes;
using Webkit.Mocking.EntityFramework;
using Webkit.Models.EntityFramework;
using Webkit.Security;
using Webkit.Security.Password;
using Webkit.Sessions;
using Webkit.Extensions;
using Webkit.Extensions.Logging;
using System.Text;
using System.Reflection;
using Webkit.Email.SendGrid;
using SendGrid.Helpers.Mail;
using Webkit.Extensions.Compression;
using Webkit.Extensions.DataConversion;
using Webkit.Data;
using System.Diagnostics;
using Webkit.Architectures.Default;

namespace Webkit.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

            DefaultArchitecturePack.Load(app, "");

            using (MockDatabase db = new MockDatabase())
            {
                string firstName = TestData.FirstName();
                string lastName = TestData.LastName();

                db.Users.Add(new UserModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = "andbjorn",
                    Email = TestData.Email(firstName, lastName),
                    Password = PasswordHandler.Hash("123"),
                    Roles = new List<string>
                    {
                        "Users",
                        "Administrator",
                    }
                });

                db.SaveChanges();
            }


            app.MapControllers();

            app.Run();
        }
    }
}
