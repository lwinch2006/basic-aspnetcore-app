using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.Configurations
{
    public class ApiConfigurationTests
    {
        [Fact]
        public void TestingApiConfiguration_ShouldPass()
        {
            var apiConfiguration = new ApiConfiguration {Url = "url1"};
            Assert.Equal("url1", apiConfiguration.Url);

            apiConfiguration.Url = "url2";
            Assert.Equal("url2", apiConfiguration.Url);
        }
    }
}