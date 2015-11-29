using Microsoft.Owin;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LanguageExchange.Security;

namespace System.Net.Http
{
    public static class HttpRequestMessageExtensions
    {
        public static bool CheckAccess(this HttpRequestMessage request, params string[] scope)
        {
            return AsyncHelper.RunSync(() => request.CheckAccessAsync(scope));
        }

        public static Task<bool> CheckAccessAsync(this HttpRequestMessage request, params string[] scope)
        {
            var user = request.GetRequestContext().Principal as ClaimsPrincipal;
            user = user ?? new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(1) { new Claim(ClaimTypes.Name, "") }));

            var ctx = new ScopeAuthorizationContext(user, scope);

            return request.CheckAccessAsync(ctx);
        }

        public static Task<bool> CheckAccessAsync(this HttpRequestMessage request, IEnumerable<Claim> scope)
        {
            var authorizationContext = new ScopeAuthorizationContext(
                request.GetOwinContext().Authentication.User ?? new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>(1) { new Claim(ClaimTypes.Name, "") })),
                scope);

            return request.CheckAccessAsync(authorizationContext);
        }

        public static Task<bool> CheckAccessAsync(this HttpRequestMessage request, ScopeAuthorizationContext authorizationContext)
        {
            return request.GetOwinContext().CheckAccessAsync(authorizationContext);
        }

        private static async Task<bool> CheckAccessAsync(this IOwinContext context, ScopeAuthorizationContext authorizationContext)
        {
            return await context.GetAuthorizationManager().CheckAccessAsync(authorizationContext);
        }

        private static IScopeAuthorizationManager GetAuthorizationManager(this IOwinContext context)
        {
            var am = context.Get<IScopeAuthorizationManager>(ScopeAuthorizationManagerMiddleware.Key);

            if (am == null)
            {
                throw new InvalidOperationException("No AuthorizationManager set.");
            }

            return am;
        }
    }
}