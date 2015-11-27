using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using LanguageExchange.Common;

namespace LanguageExchange.Security
{
    public class ApplicationClientManager : IDisposable
    {
        public readonly ApplicationDbContext Context;

        public ApplicationClientManager(ApplicationDbContext context)
        {
            Context = context;
        }

        public static ApplicationClientManager Create(IOwinContext context)
        {
            var appManager = new ApplicationClientManager(context.Get<ApplicationDbContext>());

            return appManager;
        }

        public async Task<Client> FindClient(string clientId, string clientSecret)
        {
            string hashedSecret = Hasher.GetHash(clientSecret);
            return await Context.Clients.FirstOrDefaultAsync(c => c.Name == clientId && c.Secret == hashedSecret);
        }

        public async Task AddClient(Client client)
        {
            Context.Clients.Add(client);
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
