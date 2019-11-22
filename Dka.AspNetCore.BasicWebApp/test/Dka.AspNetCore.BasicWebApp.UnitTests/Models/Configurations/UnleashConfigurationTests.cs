using Dka.AspNetCore.BasicWebApp.Models.Configurations;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.Configurations
{
    public class UnleashConfigurationTests
    {
        [Fact]
        public void TestingUnleashConfiguration_ShouldPass()
        {
            var unleashConfiguration = new UnleashConfiguration
            {
                AppName = "string-1",
                InstanceTag = "string-2",
                UnleashApi = "string-3"
            };
            
            Assert.Equal("string-1", unleashConfiguration.AppName);
            Assert.Equal("string-2", unleashConfiguration.InstanceTag);
            Assert.Equal("string-3", unleashConfiguration.UnleashApi);

            unleashConfiguration.AppName = "string-5";
            unleashConfiguration.InstanceTag = "string-6";
            unleashConfiguration.UnleashApi = "string-7";

            Assert.Equal("string-5", unleashConfiguration.AppName);
            Assert.Equal("string-6", unleashConfiguration.InstanceTag);
            Assert.Equal("string-7", unleashConfiguration.UnleashApi);
        }
    }
}