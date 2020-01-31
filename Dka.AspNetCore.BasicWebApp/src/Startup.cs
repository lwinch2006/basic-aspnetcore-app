using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Models.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

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
            AddInternalApiClient(services);
            
            services.AddHttpContextAccessor();
            services.AddUnleashClient();
            services.AddAutoMapper(typeof(BasicWebAppProfile));
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.RequireAuthenticatedSignIn = false;
                })
                .AddCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.LoginPath = AuthenticationDefaults.LoginUrl;
                    options.LogoutPath = AuthenticationDefaults.LogoutUrl;
                    options.ReturnUrlParameter = AuthenticationDefaults.ReturnUrlParameter;
                });

            services.AddAuthorization();
            
            var authorizationOptions = Options.Create(new AuthorizationOptions());
            
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForAdministrator>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForSupport>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForPowerUser>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerBasedOnRight>();
            services.AddSingleton<IAuthorizationPolicyProvider>(sp => new DataOperationAuthorizationPolicyProvider(CookieAuthenticationDefaults.AuthenticationScheme, authorizationOptions)); 
            
            services.AddControllersWithViews(config =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                
                config.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                
            }).AddRazorRuntimeCompilation();

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, ILoggerFactory loggerFactory, IHostEnvironment hostEnvironment)
        {
            loggerFactory.AddSerilog();
            
            if (hostEnvironment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/500");
                //app.UseDeveloperExceptionPage();
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
            
            app.UseEndpoints(configure =>
            {
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