using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.Configurations
{
    public class WebHostConfigurationTests
    {
        [Fact]
        public void TestingWebHostConfiguration_ShouldPass()
        {
            var webHostConfiguration = new WebHostConfiguration
            {
                Urls = new[] {"url1", "url2"}
            };
    
            Assert.Equal("url1", webHostConfiguration.Urls[0]);
            Assert.Equal("url2", webHostConfiguration.Urls[1]);

            webHostConfiguration.Urls = new[] {"url5", "url6"};

            Assert.Equal("url5", webHostConfiguration.Urls[0]);
            Assert.Equal("url6", webHostConfiguration.Urls[1]);
        }
    }
}