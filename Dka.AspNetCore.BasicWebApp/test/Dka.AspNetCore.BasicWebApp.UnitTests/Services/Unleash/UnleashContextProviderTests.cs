using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Http;
using Unleash;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class UnleashContextProviderTests
    {
        [Fact]
        public void TestingUnleashContextProvider_ShouldPass()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var unleashContextProvider = new UnleashContextProvider(httpContextAccessor);
            Assert.Null(unleashContextProvider.Context);

            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.Items["UnleashContext"] = new UnleashContext();
            unleashContextProvider = new UnleashContextProvider(httpContextAccessor);
            Assert.NotNull(unleashContextProvider.Context);
        }
    }
}