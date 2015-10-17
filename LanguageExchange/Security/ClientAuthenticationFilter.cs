using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Security.Principal;
using System.Security.Claims;

namespace LanguageExchange.Security
{
    public class ClientAuthenticationFilter : Attribute, IAuthenticationFilter, IFilter
    {
        public bool AllowMultiple
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                return;
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Bearer")
            {
                return;
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            var claimsList = new List<Claim>()
            {
                new Claim("access", "full")
            };

            ClaimsIdentity ident = new ClaimsIdentity(claimsList);
            IPrincipal principal = new ClaimsPrincipal(ident);

            if(principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }
        }
    }
}