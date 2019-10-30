using System.IO;
using System.Linq;
using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp.SystemTests
{
    public class BasicWebAppServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string EnvironmentName = "Test";

        private IWebHost _host;

        public string RootUri { get; private set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            return null;
        }   
        
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder();
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var assemblyName = typeof(TStartup).Assembly.GetName().Name;
            
            var configuration = BuildConfiguration();

            var webHostConfiguration = new WebHostConfiguration();
            configuration.GetSection($"{assemblyName}:webHost").Bind(webHostConfiguration);
            
            builder
                .UseEnvironment(EnvironmentName)
                .UseConfiguration(configuration)
                .UseStartup<TStartup>()
                .UseUrls(webHostConfiguration.Urls);
        }
        
        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            _host = builder.Build();
            _host.Start();
            RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
            
            var webHostBuilder = new WebHostBuilder();
            ConfigureWebHost(webHostBuilder);
            
            return new TestServer(webHostBuilder);
        }
        
        protected override void Dispose(bool disposing) 
        {
            base.Dispose(disposing);
            
            if (disposing) {
                _host?.Dispose();
            }
        }
        
        private IConfiguration BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{EnvironmentName}.json", true, true)
                .Build();

            return configuration;
        }
    }
}