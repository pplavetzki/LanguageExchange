namespace LanguageExchange.DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using Common;
    using System.Configuration;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Digests;
    using System.Text;

    internal sealed class AuthConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public AuthConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var userStore = new ApplicatonUserStore(context);
            var manager = new ApplicationUserManager(userStore);

            var cipher = ConfigurationManager.AppSettings["cipherKey"];
            var hmacKey = ConfigurationManager.AppSettings["hmacKey"];

            byte[] key = Convert.FromBase64String(cipher);
            byte[] hmac = Convert.FromBase64String(hmacKey);

            //var encryptor = new Encryptor<AesEngine, Sha256Digest>(Encoding.UTF8, key, hmac);

            var applicationRole = new ApplicationRole()
            {
                Name = "Administrator"
            };

            context.Roles.Add(applicationRole);
            var createRole = context.Roles.FirstOrDefault(r => r.Name == "Administrator");

            context.Roles.Add(new ApplicationRole() { Name = "User" });

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
                UserName = "pplavetzki",
                SocialSecurityNumber = AESGCM.SimpleEncrypt("555555555", key, hmac)
            };

            var result = manager.Create(user, "HugeWin12!");

            if (result.Succeeded)
            {
                var createdUser = context.Users.FirstOrDefault(u => u.Firstname == "Paul");
                manager.AddToRole(createdUser.Id, "Administrator");


            }
        }
    }
}
