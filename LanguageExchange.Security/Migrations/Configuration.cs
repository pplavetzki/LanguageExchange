namespace LanguageExchange.Security.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override async void Seed(ApplicationDbContext context)
        {
            var userStore = new ApplicatonUserStore(context);
            var manager = new ApplicationUserManager(userStore);

            //var encryptor = new Encryptor<AesEngine, Sha256Digest>(Encoding.UTF8, key, hmac);

            var applicationRole = new IdentityRole()
            {
                Name = "Administrator"
            };

            context.Roles.Add(applicationRole);
            var createRole = context.Roles.FirstOrDefault(r => r.Name == "Administrator");

            context.Roles.Add(new IdentityRole() { Name = "User" });

            var user = new ApplicationUser()
            {
                Firstname = "Paul",
                Lastname = "Plavetzki",
                Email = "paul@p2the3.com",
                AccessFailedCount = 0,
                EmailConfirmed = false,
                JoinDate = DateTime.UtcNow,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                UserName = "pplavetzki"
            };

            var result = await manager.CreateAsync(user, "HugeWin12!");

            if (result.Succeeded)
            {
                await manager.AddToRoleAsync(user.Id, "Administrator");
            }

        }
    }
}
