using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Unleash;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class UnleashServiceCollectionExtensionTests
    {
        [Fact]
        public void TestingUnleashServiceCollectionExtension_ShouldPass()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            Assert.Null(serviceProvider.GetService<EnvironmentNameStrategy>());
            Assert.Null(serviceProvider.GetService<TenantGuidStrategy>());
            Assert.Null(serviceProvider.GetService<UnleashContextProvider>());
            
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddUnleashClient();
            serviceProvider = services.BuildServiceProvider();
            
            Assert.NotNull(serviceProvider.GetService<EnvironmentNameStrategy>());
            Assert.NotNull(serviceProvider.GetService<TenantGuidStrategy>());
            Assert.NotNull(serviceProvider.GetService<IUnleashContextProvider>());
        }
    }
}