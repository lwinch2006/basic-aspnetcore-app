using System.Diagnostics.CodeAnalysis;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp
{
    [ExcludeFromCodeCoverage]
    public class StartupTest : Startup
    {
        public StartupTest(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        { }
        
        protected override void AddInternalApiClient(IServiceCollection services)
        {
            services.AddHttpClient<IInternalApiClient, InternalApiClientTest>();
        }
    }
}