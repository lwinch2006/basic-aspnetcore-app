using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Dka.AspNetCore.BasicWebApp.Api
{
    public class StartupTest : Startup
    {
        public StartupTest(IConfiguration configuration, IHostEnvironment environment)
            : base(configuration, environment) 
        {
        }
    }
}