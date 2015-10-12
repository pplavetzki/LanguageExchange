using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace LanguageExchange.Security
{
    public class LanguageExchangeAuthorization : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == "role");
            if (claim != null)
            {
                if (claim.Value == "Administrator")
                {
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
    }
}