using System;
using Dka.AspNetCore.BasicWebApp.Configurations;
using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dka.AspNetCore.BasicWebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var apiConfiguration = new ApiConfiguration();
            _configuration.GetSection("api").Bind(apiConfiguration);
            
            services.AddHttpClient();
            services.AddHttpClient<InternalApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiConfiguration.Url);
            });
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
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapRazorPages();
            });
        }
    }
}