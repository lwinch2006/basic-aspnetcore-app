using Microsoft.Extensions.DependencyInjection;
using Unleash;
using Unleash.Strategies;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public static class UnleashServiceCollectionExtension
    {
        public static void AddUnleashClient(this IServiceCollection services)
        {
            services.AddScoped<EnvironmentNameStrategy>();
            services.AddScoped<TenantGuidStrategy>();
            services.AddScoped<IUnleashContextProvider, UnleashContextProvider>();
            services.AddScoped<UnleashClient>();
        }
    }
}