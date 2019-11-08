using System;
using System.Net.Http;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Models.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

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
            services.AddUnleashClient();
            services.AddAutoMapper(typeof(BasicWebAppProfile));
            
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            logger.LogInformation("Web application initialised");

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseUnleashMiddleware();
            
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapRazorPages();
            });
        }

        protected virtual void AddInternalApiClient(IServiceCollection services)
        {
            var apiConfiguration = new ApiConfiguration();
            _configuration.GetSection($"{_applicationName}:api").Bind(apiConfiguration);
            
            services.AddHttpClient<IInternalApiClient, InternalApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiConfiguration.Url);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                // Important to return new http handler each time http client created/invoked.
                var httpClientHandler = new HttpClientHandler();
            
                if (EnvironmentLogic.IsDevelopment())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; 
                }

                return httpClientHandler;
            });
        }
    }
}