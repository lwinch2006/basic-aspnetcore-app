using Dka.AspNetCore.BasicWebApp.Api.Models.Configurations;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Models.Configurations
{
    public class WebHostConfigurationTests
    {
        [Fact]
        public void TestingWebHostConfiguration_GettingSettingProperties_ShouldPass()
        {
            var webHostConfiguration = new WebHostConfiguration { Urls = new[] {"url1", "url2"}};
            
            Assert.NotNull(webHostConfiguration);
            Assert.NotNull(webHostConfiguration.Urls);
            Assert.Equal("url1", webHostConfiguration.Urls[0]);
            Assert.Equal("url2", webHostConfiguration.Urls[1]);
        }
    }
}