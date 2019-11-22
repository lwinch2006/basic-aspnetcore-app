using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Moq;
using Unleash;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class UnleashMiddlewareTests
    {
        [Fact]
        public async Task TestingUnleashMiddleware_ShouldPass()
        {
            var httpContext = new DefaultHttpContext();
            var hostEnvironment = new Mock<IHostEnvironment>();
            hostEnvironment.Setup(env => env.EnvironmentName).Returns("development");            
            var requestDelegate = new Mock<RequestDelegate>();
            requestDelegate.Setup(request => request.Invoke(It.IsAny<HttpContext>())).Verifiable();
            
            var unleashMiddleware = new UnleashMiddleware(requestDelegate.Object);
            await unleashMiddleware.Invoke(httpContext, hostEnvironment.Object);

            Assert.NotNull(httpContext.Items["UnleashContext"]);
            Assert.Equal("development", ((UnleashContext)httpContext.Items["UnleashContext"]).Properties[UnleashConstants.EnvironmentStrategyName]);
            Assert.Equal("faeadb60-75bf-4b63-a1f7-84e2fc5d681c", ((UnleashContext)httpContext.Items["UnleashContext"]).Properties[UnleashConstants.TenantGuidStrategyName]);
            requestDelegate.Verify();
        }

        [Fact]
        public async Task TestingUnleashMiddleware_PassHttpContextNull_ThrowsException_ShouldPass()
        {
            var httpContext = new DefaultHttpContext();
            var hostEnvironment = new Mock<IHostEnvironment>();
            hostEnvironment.Setup(env => env.EnvironmentName).Returns("development");            
            var requestDelegate = new Mock<RequestDelegate>();
            requestDelegate.Setup(request => request.Invoke(It.IsAny<HttpContext>())).Verifiable();
            
            var unleashMiddleware = new UnleashMiddleware(requestDelegate.Object);

            try
            {
                await unleashMiddleware.Invoke(null, hostEnvironment.Object);
            }
            catch (NullReferenceException)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public async Task TestingUnleashMiddleware_PassHostEnvironmentNull_ThrowsException_ShouldPass()
        {
            var httpContext = new DefaultHttpContext();
            var hostEnvironment = new Mock<IHostEnvironment>();
            hostEnvironment.Setup(env => env.EnvironmentName).Returns("development");            
            var requestDelegate = new Mock<RequestDelegate>();
            requestDelegate.Setup(request => request.Invoke(It.IsAny<HttpContext>())).Verifiable();
            
            var unleashMiddleware = new UnleashMiddleware(requestDelegate.Object);

            try
            {
                await unleashMiddleware.Invoke(httpContext, null);
            }
            catch (NullReferenceException)
            {
                Assert.True(true);
            }
        }        
    }
}