using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Configurations;
using Dka.AspNetCore.BasicWebApp.SeleniumTests.Services;
using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Dka.AspNetCore.BasicWebApp.SeleniumTests
{
    public class BasicWebAppSeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string EnvironmentName = "Test";

        private readonly Process _process;
        private IWebHost _host;

        public string RootUri { get; set; }
        
        public BasicWebAppSeleniumServerFactory()
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
                .UseStartup<Startup>()
                .UseUrls(webHostConfiguration.Urls)
                .ConfigureTestServices(testServices =>
                {
                    testServices.AddHttpClient<IInternalApiClient, TestInternalApiClient>();
                });
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