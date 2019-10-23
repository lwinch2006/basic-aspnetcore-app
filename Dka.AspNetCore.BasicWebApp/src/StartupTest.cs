using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dka.AspNetCore.BasicWebApp
{
    public class StartupTest : Startup
    {
        public StartupTest(IConfiguration configuration)
            : base(configuration)
        { }
        
        protected override void AddInternalApiClient(IServiceCollection services)
        {
            services.AddHttpClient<IInternalApiClient, InternalApiClientTest>();
        }
    }
}