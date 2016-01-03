using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using LanguageExchange.Services;
using System.Web;

namespace LanguageExchange.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IApplicationUserStore store) : base(store) { }

        public async Task SendConfirmationEmail(string userId, string email)
        {
            var code = await GenerateEmailConfirmationTokenAsync(userId);
            var encodedToken = HttpUtility.UrlEncode(code);

            string callback = "http://localhost:9095/confirm?userId={0}&code={1}";

            var url = string.Format(callback, userId, encodedToken);
            string message = "Please click this link or paste into a browser: <a href='" + url + "'>" + url + "</a>";

            await EmailService.SendAsync(new IdentityMessage() { Subject = "You've been Registered.", Destination = email, Body = message });
        }

        public async Task QuickRegistration(ApplicationUser user, string password)
        {
            var result = await CreateAsync(user, password);
            if (result.Succeeded)
            {
                await SendConfirmationEmail(user.Id, user.Email);
            }
            else
            {
                throw new Exception("Failed to Create User");
            }
        }

        public static ApplicationUserManager Create(
        IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new ApplicatonUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            
            manager.EmailService = new MailTrapService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(
                        dataProtectionProvider.Create("ASP.NET Identity"))
                            { TokenLifespan = TimeSpan.FromDays(1) };
            }
            return manager;
        }
    }
}
