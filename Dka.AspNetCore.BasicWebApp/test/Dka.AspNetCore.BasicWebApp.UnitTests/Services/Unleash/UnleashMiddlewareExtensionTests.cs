using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class UnleashMiddlewareExtensionTests
    {
        [Fact]
        public void TestingUnleashMiddlewareExtension_ShouldPass()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            var appBuilder = (IApplicationBuilder)new ApplicationBuilder(serviceProvider);

            appBuilder = appBuilder.UseUnleashMiddleware();
            
            Assert.NotNull(appBuilder);
        }
    }
}