using LanguageExchange.Security;

namespace Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseScopeAuthentication(this IAppBuilder app, IScopeAuthorizationManager authManager)
        {
            var options = new ScopeAuthorizationMiddlewareOptions
            {
                Manager = authManager
            };

            app.Use(typeof(ScopeAuthorizationManagerMiddleware), options);
            return app;
        }
    }
}
