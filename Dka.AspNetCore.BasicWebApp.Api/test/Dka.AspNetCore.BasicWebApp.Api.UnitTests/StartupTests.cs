using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests
{
    public class StartupTests
    {
        [Fact]
        public void TestingStartupClass_ShouldPass()
        {
            // TODO: Make real testing here instead of fake.

            var configuration = new Mock<IConfiguration>(); 
            var environment = new Mock<IHostEnvironment>();
            
            var startup = new Startup(configuration.Object, environment.Object);
            var services = new ServiceCollection();
            
            Assert.NotNull(startup);
        }
    }
}