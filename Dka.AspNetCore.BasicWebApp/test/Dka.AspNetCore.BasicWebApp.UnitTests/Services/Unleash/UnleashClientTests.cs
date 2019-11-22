using System.IO;
using System.Text;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class UnleashClientTests
    {
        [Fact]
        public void TestingUnleashClient_ShouldPass()
        {
            const string configurationAsJsonString = @"
                {
                    ""Dka.AspNetCore.BasicWebApp"": {
                        ""Unleash"": {
                            ""AppName"": ""Dka.AspNetCore.BasicWebApp"",
                            ""InstanceTag"": ""prod"",
                            ""UnleashApi"": ""http://localhost:4242""
                        } 
                    }
                }
            ";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(configurationAsJsonString));
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonStream(stream);
            var configuration = configurationBuilder.Build();

            var unleashClient = new UnleashClient(configuration, new UnleashContextProvider(new HttpContextAccessor()), new EnvironmentNameStrategy(), new TenantGuidStrategy());     

            Assert.NotNull(unleashClient);
            Assert.False(unleashClient.AdministrationMenuEnabled);
        }
    }
}