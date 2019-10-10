using System;
using System.IO;
using Dka.AspNetCore.BasicWebApp.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
            var configuration = BuildConfiguration();

            var webHostConfiguration = new WebHostConfiguration();
            configuration.GetSection("webHost").Bind(webHostConfiguration);
            
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
            return CreateHostBuilder(args).Build();
        }

        private static IConfiguration BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production" }.json", true, true)
                .Build();

            return configuration;
        }

        private static void ConfigureLogging()
        {
            
            
            
            
        }
    }
}