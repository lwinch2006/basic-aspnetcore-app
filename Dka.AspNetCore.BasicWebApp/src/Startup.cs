using System;
using System.Net.Http;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp
{
    public class Startup
    {
        protected readonly string _applicationName;
        
        protected readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            _applicationName = env.ApplicationName;
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            AddInternalApiClient(services);

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            logger.LogInformation("Web application initialised");
            
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseStaticFiles();
            
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("administration", "Administration/{controller=Home}/{action=Index}/{id?}");
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapRazorPages();
            });
            
            
        }

        protected virtual void AddInternalApiClient(IServiceCollection services)
        {
            var apiConfiguration = new ApiConfiguration();
            _configuration.GetSection($"{_applicationName}:api").Bind(apiConfiguration);
            
            var httpClientHandler = new HttpClientHandler();
            
            if (EnvironmentLogic.IsDevelopment())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; 
            }
            
            services.AddHttpClient<IInternalApiClient, InternalApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiConfiguration.Url);
            }).ConfigurePrimaryHttpMessageHandler(() => httpClientHandler);
        }
    }
}