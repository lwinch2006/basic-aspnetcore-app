using Dka.AspNetCore.BasicWebApp.Api.Models.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Models.AutoMapper
{
    public class BasicWebAppProfileTests
    {
        [Fact]
        public void TestingBasicWebAppProfile_ShouldPass()
        {
            var basicWebAppApiProfile = new BasicWebAppApiProfile();
            Assert.NotNull(basicWebAppApiProfile);
        }
    }
}