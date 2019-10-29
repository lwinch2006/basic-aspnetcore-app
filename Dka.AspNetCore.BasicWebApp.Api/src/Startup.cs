using Dka.AspNetCore.BasicWebApp.Api.Extensions;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                configure.MapRazorPages();
            });
        }        
    }
}