using JiraClone.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();
            //var jWTServices = serviceScope.ServiceProvider.GetService<IJWTServices>();
            //jWTServices.GetService();
            var context = serviceScope.ServiceProvider.GetService<JiraCloneDbContext>();


            if (!context.IdentityClients.Any())
            {
                context.IdentityClients.Add(new IdentityClient()
                {
                    IdentityClientId = "EPS",
                    Description = "EPS",
                    SecretKey = "b0udcdl8k80cqiyt63uq",
                    ClientType = 0,
                    IsActive = true,
                    RefreshTokenLifetime = 30,
                    AllowedOrigin = "*"
                });

                context.SaveChanges();

                var passwordHasher = new PasswordHasher<User>();
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "admin@gmail.com",
                    FullName = "Quản trị hệ thống",
                    NormalizedUserName = "admin",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Status = 2,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsAdministrator = true,
                    GroupUsers = new List<GroupUser>(),
                    UserDetails = new List<UserDetail>()
                };

                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456a@");
                context.Users.Add(adminUser);
                context.SaveChanges();
                adminUser.GroupUsers.Add(new GroupUser() { GroupId = 1, UserId = 1 });
                context.SaveChanges();
                adminUser.UserDetails.Add(new UserDetail { UserId = 1 });
                context.SaveChanges();
            }
        }
    }
}
