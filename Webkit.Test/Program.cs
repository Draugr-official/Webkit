
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Webkit.Attributes;
using Webkit.Mocking.EntityFramework;
using Webkit.Models.EntityFramework;
using Webkit.Security;
using Webkit.Security.Password;
using Webkit.Sessions;

namespace Webkit.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using(Database db = new Database())
            //{
            //    List<User> users = db.Users.Where(user => user.Id == Database.Test1UserId).ToList();
            //    if(!users.Any())
            //    {
            //        Console.WriteLine("No users found");
            //        return;
            //    }

            //    User user = users.First();
            //}

            AuthenticateAttribute.ValidateToken = bool (string token) =>
            {
                using (MockDatabase db = new MockDatabase())
                {
                    return db.Users.Any(user => user.Token == token);
                }
            };

            string passwordHash = PasswordHandler.Hash("123abc");
            Console.WriteLine("Thingy: " + passwordHash);

            Console.WriteLine("Matches? " + PasswordHandler.Validate("123abc", passwordHash));
            
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
