using Microsoft.AspNetCore.Builder;
using OrchardCore.Ldap.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLdapMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LdapMiddleware>();
            return app;
        }
    }
}
