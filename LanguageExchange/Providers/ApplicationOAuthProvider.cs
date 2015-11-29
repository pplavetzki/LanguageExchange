using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LanguageExchange.Models;
using LanguageExchange.Security;

namespace LanguageExchange.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            string userId = "";

            string scope = context.OwinContext.Get<string>("as:Scope");
            string allowedOrigin = context.OwinContext.Get<string>("as:AllowedOrigin");

            if (string.IsNullOrWhiteSpace(scope))
            {
                context.SetError("invalid_scope", "The scope is invalid.");
                return;
            }

            ApplicationUser user = await userManager.FindByNameAsync(context.UserName);
           
            if(user != null)
            {
                userId = user.Id;
                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "Email is invalid.");
                    return;
                }

                user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    await userManager.AccessFailedAsync(userId);
                  
                    return;
                }
            }
            else
            {
                context.SetError("invalid_grant", "The user is invalid.");
                return;
            }
            
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim("scope", scope));

            //ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
            //    CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(context.ClientId);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            ApplicationClientManager cm = ApplicationClientManager.Create(context.OwinContext);
            Client client = null;
            
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
                return;
            }

            client = await cm.FindClient(clientId, clientSecret);
            if (client == null)
            {
                context.SetError("invalid_client", "No Client");
                return;
            }

            if(!client.Active)
            {
                context.SetError("invalid_client", "Not Active");
                return;
            }

            context.OwinContext.Set("as:AllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:RefreshTokenLifetime", client.RefreshTokenLifeTime.ToString());
            context.OwinContext.Set("as:Scope", client.Scope);

            context.Validated();
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string clientId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "as:client_id", clientId }
            };
            return new AuthenticationProperties(data);
        }
    }
}