using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Controllers;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Controllers
{
    public class AboutControllerTests
    {
        private (AboutController, Mock<IInternalApiClient>) SetupController()
        {
            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            var logger = new Mock<ILogger<AboutController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            internalApiClient.Setup(client => client.GetPageNameAsync(It.IsAny<string>())).Returns<string>(pageName =>
                {
                    return Task.FromResult(pageName);
                }).Verifiable();
            
            var aboutController = new AboutController(internalApiClient.Object, httpContextAccessor, logger.Object);

            return (aboutController, internalApiClient);
        }
        
        private (AboutController, Mock<IInternalApiClient>) SetupControllerWithThrowingException()
        {
            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            var logger = new Mock<ILogger<AboutController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            internalApiClient.Setup(client => client.GetPageNameAsync(It.IsAny<string>())).Returns<string>(pageName => throw new ApiConnectionException()).Verifiable();
            
            var aboutController = new AboutController(internalApiClient.Object, httpContextAccessor, logger.Object);

            return (aboutController, internalApiClient);
        }

        [Fact]
        public async Task TestingIndexAction_ShouldPass()
        {
            var (aboutController, internalApiClient) = SetupController();

            var result = await aboutController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("About", viewResult.ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi]);
            internalApiClient.Verify();
        }

        [Fact]
        public async Task TestingIndexAction_ThrowingException_ShouldPass()
        {
            var (aboutController, internalApiClient) = SetupControllerWithThrowingException();

            var result = await aboutController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(string.Empty, viewResult.ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi]);
            internalApiClient.Verify();
        }
    }
}