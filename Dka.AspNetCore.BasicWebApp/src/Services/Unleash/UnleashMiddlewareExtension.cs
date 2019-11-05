using Microsoft.AspNetCore.Builder;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public static class UnleashMiddlewareExtension
    {
        public static IApplicationBuilder UseUnleashMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnleashMiddleware>();
        }
    }
}