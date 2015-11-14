using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Services
{
    public class MailTrapService : IIdentityMessageService, IDisposable
    {
        public Task SendAsync(IdentityMessage message)
        {
            var client = new SmtpClient("mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("481254c2e0b2042a0", "3ffec4683a761a"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage("registration@pareidoliasw.com", message.Destination, message.Subject, message.Body);
            mailMessage.IsBodyHtml = true;

            client.Send(mailMessage);
            //client.SendAsync(mailMessage, null);

            return Task.FromResult(0);
        }

        public static MailTrapService Create()
        {
            var mailTrap = new MailTrapService();

            return mailTrap;
        }

        public void Dispose()
        {
            
        }
    }
}
