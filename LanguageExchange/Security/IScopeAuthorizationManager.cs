using System.Threading.Tasks;

namespace LanguageExchange.Security
{
    public interface IScopeAuthorizationManager
    {
        Task<bool> CheckAccessAsync(ScopeAuthorizationContext context);
    }
}
