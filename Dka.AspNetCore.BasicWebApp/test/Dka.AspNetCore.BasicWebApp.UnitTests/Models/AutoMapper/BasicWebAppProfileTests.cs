using Dka.AspNetCore.BasicWebApp.Models.AutoMapper;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.AutoMapper
{
    public class BasicWebAppProfileTests
    {
        [Fact]
        public void TestingBasicWebAppProfile_ShouldPass()
        {
            var basicWebAppProfile = new BasicWebAppProfile();
            Assert.NotNull(basicWebAppProfile);
        }
    }
}