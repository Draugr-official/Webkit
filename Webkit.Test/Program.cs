
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

namespace Webkit.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for(int i = 0; i < 50; i++)
            {
                string firstName = TestData.FirstName();
                string lastName = TestData.LastName();

                string domain = TestData.Domain(firstName);
                string email = TestData.Email(firstName, lastName, domain);

                Console.WriteLine($"{firstName} {lastName}, {domain}, {email}");
            }

            return;
            AuthenticateAttribute.Validate = bool (string token) =>
            {
                using (MockDatabase db = new MockDatabase())
                {
                    return db.Users.Any(user => user.Token == token);
                }
            };
            Console.OutputEncoding = Encoding.Unicode;
            CryptographicGenerator.UnicodeSeed(40).Log();

            string[] strArr = ["ah"];
            strArr.Log();

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


            app.MapControllers();

            app.Run();
        }
    }
}
