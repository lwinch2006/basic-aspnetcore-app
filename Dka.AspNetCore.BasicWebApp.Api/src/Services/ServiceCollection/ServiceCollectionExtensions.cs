using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Dka.AspNetCore.BasicWebApp.Api.Services.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseClasses(this IServiceCollection services, DatabaseConfiguration databaseConfiguration)
        {
            services.AddScoped(sp => new DatabaseConnectionFactory(databaseConfiguration));
            
            // Repositories.
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<RoleRepository>();

            // Logic.
            services.AddScoped<ITenantLogic, TenantLogic>();
            services.AddScoped<ApplicationUserStore>();
            services.AddScoped<ApplicationRoleStore>();
        }
    }
}