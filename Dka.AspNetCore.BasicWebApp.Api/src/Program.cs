using System;
using System.IO;
using Dka.AspNetCore.BasicWebApp.Api.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            webHost.Run();
        }
        
        private static IHost BuildWebHost(string[] args)
        {
            var configuration = BuildConfiguration();

            var webHostConfiguration = new WebHostConfiguration();
            configuration.GetSection("webHost").Bind(webHostConfiguration);
            
            var webHost = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder.UseConfiguration(configuration);
                    webHostBuilder.UseStartup<Startup>();
                    webHostBuilder.UseUrls(webHostConfiguration.Urls);
                })
                .Build();

            return webHost;
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