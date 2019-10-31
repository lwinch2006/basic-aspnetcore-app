using System;
using System.IO;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var assemblyName = typeof(Startup).Assembly.GetName().Name;
            
            var configuration = BuildConfiguration();

            var webHostConfiguration = new WebHostConfiguration();
            configuration.GetSection($"{assemblyName}:webHost").Bind(webHostConfiguration);
            
            var webHostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostConfigurator =>
                {
                    webHostConfigurator.UseConfiguration(configuration);
                    webHostConfigurator.UseStartup<Startup>();
                    webHostConfigurator.UseUrls(webHostConfiguration.Urls);
                });

            return webHostBuilder;
        }

        private static IHost BuildWebHost(string[] args)
        {
            // Configuring web host defaults.
            var hostBuilder = CreateHostBuilder(args);
            
            // Configuring web host logging.
            ConfigureLogging(hostBuilder);
            
            // Building web host.
            return hostBuilder.Build();
        }

        private static IConfiguration BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName) ?? "Production" }.json", true, true)
                .Build();

            return configuration;
        }

        private static void ConfigureLogging(IHostBuilder hostBuilder)
        {
            var assemblyName = typeof(Startup).Assembly.GetName().Name;
            var configuration = BuildConfiguration();
            
            hostBuilder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection($"{assemblyName}:Logging"));
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });
        }
    }
}