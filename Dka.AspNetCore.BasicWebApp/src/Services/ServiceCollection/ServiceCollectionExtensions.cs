using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dka.AspNetCore.BasicWebApp.Services.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection InsertAuthentication(this IServiceCollection services)
        {
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
                    options.Events.OnRedirectToAccessDenied = context => {
                        context.Response.StatusCode = 403;
                        
                        return Task.CompletedTask;
                    };
                });

            return services;
        }

        public static IServiceCollection InsertAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            
            var authorizationOptions = Options.Create(new AuthorizationOptions());
            
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForAdministrator>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForSupport>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForPowerUser>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerBasedOnRight>();
            services.AddSingleton<IAuthorizationPolicyProvider>(sp => new DataOperationAuthorizationPolicyProvider(CookieAuthenticationDefaults.AuthenticationScheme, authorizationOptions));

            return services;
        }

        public static IMvcBuilder InsertControllers(this IServiceCollection services)
        {
            var mvsBuilder = services.AddMvc(config =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(authorizationPolicy));
            }).AddRazorRuntimeCompilation();

            return mvsBuilder;
        }

        public static IServiceCollection InsertInternalApiClient(this IServiceCollection services, IConfiguration configuration, string applicationName)
        {
            var apiConfiguration = new ApiConfiguration();
            configuration.GetSection($"{applicationName}:api").Bind(apiConfiguration);
            
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

            return services;
        }

        public static IMvcBuilder InsertLocalization(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });            
            
            mvcBuilder
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            return mvcBuilder;
        }
    }
}