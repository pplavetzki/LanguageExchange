using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Security
{
    public class ScopeAuthorizationManagerMiddleware
    {
        public const string Key = "idm:scopeAuthorizationManager";

        private readonly Func<IDictionary<string, object>, Task> _next;
        private ScopeAuthorizationMiddlewareOptions _options;

        public ScopeAuthorizationManagerMiddleware(Func<IDictionary<string, object>, Task> next, ScopeAuthorizationMiddlewareOptions options)
        {
            _options = options;
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            env[Key] = _options.Manager ?? _options.ManagerProvider(env);
            await _next(env);
        }
    }
}
