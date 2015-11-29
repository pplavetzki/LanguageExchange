using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Security
{
    public class ScopeAuthorizationManager : IScopeAuthorizationManager
    {
        public Task<bool> CheckAccessAsync(ScopeAuthorizationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
