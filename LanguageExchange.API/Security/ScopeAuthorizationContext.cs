using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Security
{
    public class ScopeAuthorizationContext
    {
        public IEnumerable<Claim> Scope { get; set; }
        public ClaimsPrincipal Principal { get; set; }

        public ScopeAuthorizationContext(ClaimsPrincipal principal, IEnumerable<Claim> scope)
        {
            if(principal == null)
            {
                throw new ArgumentNullException("Principal");
            }
            if(scope == null)
            {
                throw new ArgumentNullException("Scope");
            }

            this.Principal = principal;
            this.Scope = scope;
        }

        public ScopeAuthorizationContext(ClaimsPrincipal principal, params string[] scope)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("Principal");
            }
            if (scope == null || scope.Length == 0)
            {
                throw new ArgumentNullException("Scope");
            }

            Scope = new List<Claim>(from s in scope select new Claim("name", s));
            Principal = principal;
        }

    }
}
