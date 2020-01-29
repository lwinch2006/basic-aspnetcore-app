using System.Net;
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
    public class HomeControllerTests
    {
        private (HomeController, Mock<IInternalApiClient>) SetupController()
        {
            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            var logger = new Mock<ILogger<HomeController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            
            var homeController = new HomeController(internalApiClient.Object, httpContextAccessor, logger.Object);

            return (homeController, internalApiClient);
        }
        
        private (HomeController, Mock<IInternalApiClient>) SetupControllerWithThrowingException()
        {
            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            var logger = new Mock<ILogger<HomeController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            
            var homeController = new HomeController(internalApiClient.Object, httpContextAccessor, logger.Object);

            return (homeController, internalApiClient);
        }        
        
        [Fact]
        public async Task TestingIndexAction_ShouldPass()
        {
            var (homeController, internalApiClient) = SetupController();

            var result = await homeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Home", viewResult.ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi]);
        }
        
        [Fact]
        public async Task TestingIndexAction_ThrowingException_ShouldPass()
        {
            var (homeController, internalApiClient) = SetupControllerWithThrowingException();

            var result = await homeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(string.Empty, viewResult.ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi]);
        }
    }
}