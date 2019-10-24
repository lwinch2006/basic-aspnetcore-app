using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp
{
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