using System;
using System.Collections.Generic;

namespace LanguageExchange.Security
{
    public class ScopeAuthorizationMiddlewareOptions
    {
        public ScopeAuthorizationMiddlewareOptions()
        {
            ManagerProvider = (env) => null;
        }
        public IScopeAuthorizationManager Manager { get; set; }
        public Func<IDictionary<string, object>, IScopeAuthorizationManager> ManagerProvider { get; set; }
    }
}
