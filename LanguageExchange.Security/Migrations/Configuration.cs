namespace LanguageExchange.Security.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExchange.Common;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private ApplicationDbContext _context;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();
        }

        private async Task Seed()
        {
            var userStore = new ApplicatonUserStore(_context);
            var manager = new ApplicationUserManager(userStore);
            var clientManager = new ApplicationClientManager(_context);

            //var encryptor = new Encryptor<AesEngine, Sha256Digest>(Encoding.UTF8, key, hmac);

            var applicationRole = new IdentityRole()
            {
                Name = "Administrator"
            };

            _context.Roles.Add(applicationRole);
            var createRole = _context.Roles.FirstOrDefault(r => r.Name == "Administrator");

            _context.Roles.Add(new IdentityRole() { Name = "User" });

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

            var client = new Client()
            {
                Active = true,
                Id = Guid.NewGuid().ToString(),
                Name = "LangExApp",
                Secret = Hasher.GetHash("6T9+akYtzZVwOztXmkso/Jrg0FvuEp0KdtZvQ+bFeLc="),
                RefreshTokenLifeTime = 2400,
                AllowedOrigin = "*",
                Scope = "full"
            };

            await clientManager.AddClient(client);
           
        }

        protected override void Seed(ApplicationDbContext context)
        {
            _context = context;
            Seed().Wait();
        }
    }
}
