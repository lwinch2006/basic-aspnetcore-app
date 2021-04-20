using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Models.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ApplicationBuilder;
using Dka.AspNetCore.BasicWebApp.Services.Pagination;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Dka.AspNetCore.BasicWebApp.Services.ServiceCollection;

namespace Dka.AspNetCore.BasicWebApp
{
    [ExcludeFromCodeCoverage]
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
            services.InsertInternalApiClient(_configuration, _applicationName);
            
            services.AddHttpContextAccessor();

            services.AddUnleashClient();
            
            services.AddAutoMapper(typeof(BasicWebAppProfile));

            services.InsertAuthentication();

            services.InsertAuthorization();

            //services.InsertLocalization();
            
            services
                .InsertControllers()
                .InsertLocalization();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, ILoggerFactory loggerFactory, IHostEnvironment hostEnvironment)
        {
            loggerFactory.AddSerilog();

            app.InsertLocalization(new[] {"en", "ru"});
            
            if (hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseHsts();
            }
            
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseUnleashMiddleware();
            app.UsePaginationMiddleware();
            
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapRazorPages();
            });
            
            logger.LogInformation("Web application initialised");
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