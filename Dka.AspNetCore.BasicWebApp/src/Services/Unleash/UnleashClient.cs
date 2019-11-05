using System;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Unleash;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public class UnleashClient
    {
        private readonly DefaultUnleash _internalUnleashClient;
        
        public bool AdministrationMenuEnabled => _internalUnleashClient?.IsEnabled("BasicWebApi.AdministrationMenuEnabled") ?? false;

        public UnleashClient(
            IConfiguration configuration, 
            IUnleashContextProvider unleashContextProvider, 
            EnvironmentNameStrategy environmentNameStrategy, 
            TenantGuidStrategy tenantGuidStrategy)
        {
            var assemblyName = typeof(Startup).Assembly.GetName().Name;
            
            var unleashConfiguration = new UnleashConfiguration();
            configuration.GetSection($"{assemblyName}:Unleash").Bind(unleashConfiguration);
            
            var unleashSettings = new UnleashSettings
            {
                AppName = unleashConfiguration.AppName,
                InstanceTag = unleashConfiguration.InstanceTag,
                UnleashApi = new Uri(unleashConfiguration.UnleashApi),
                UnleashContextProvider = unleashContextProvider
            };            
            
            _internalUnleashClient = new DefaultUnleash(
                unleashSettings,
                environmentNameStrategy,
                tenantGuidStrategy
            );
        }
    }
}