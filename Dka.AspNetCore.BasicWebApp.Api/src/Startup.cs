using Dka.AspNetCore.BasicWebApp.Api.Extensions;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp.Api
{
    public class Startup
    {
        private readonly string _appName;
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _appName = environment.ApplicationName;
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConfiguration = new DatabaseConfiguration();
            _configuration.GetSection($"{_appName}:BaseWebAppContext").Bind(databaseConfiguration);
            
            services.AddSingleton(databaseConfiguration);
            services.AddDatabaseClasses(databaseConfiguration);
            services
                .AddHealthChecks()
                .AddSqlServer(databaseConfiguration.ConnectionString, null, "Database", null, new[] {"db-status-check"}, null)
                .AddCheck("Api", () => HealthCheckResult.Healthy(), new [] {"api-status-check"})
                .AddCheck("Memory", () => HealthCheckResult.Healthy(), new [] { "memory-status-check" });
            
            
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("administration", "Administration/{controller=Home}/{action=Index}/{id?}");
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapHealthChecks("/health");
                configure.MapHealthChecks("/health/api", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("api-status-check")
                });                
                configure.MapHealthChecks("/health/database", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("db-status-check")
                });
                configure.MapHealthChecks("/health/memory", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("memory-status-check")
                });
                configure.MapRazorPages();
            });
        }        
    }
}