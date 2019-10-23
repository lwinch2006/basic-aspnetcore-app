using System.Diagnostics;
using System.IO;
using System.Linq;
using Dka.AspNetCore.BasicWebApp.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp.SeleniumTests
{
    public class BasicWebAppServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string EnvironmentName = "Test";

        private readonly Process _process;
        
        private IWebHost _host;

        public string RootUri { get; private set; }
        
        public BasicWebAppServerFactory()
        {
            _process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = "selenium-standalone",
                    Arguments = "start",
                    UseShellExecute = true
                }
            };
            
            _process.Start();            
        }

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
            var configuration = BuildConfiguration();

            var webHostConfiguration = new WebHostConfiguration();
            configuration.GetSection("webHost").Bind(webHostConfiguration);
            
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
            
            return new TestServer(new WebHostBuilder().UseStartup<TStartup>());
        }
        
        protected override void Dispose(bool disposing) 
        {
            base.Dispose(disposing);
            
            if (disposing) {
                _host?.Dispose();
                _process?.CloseMainWindow();
                _process?.Close(); 
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