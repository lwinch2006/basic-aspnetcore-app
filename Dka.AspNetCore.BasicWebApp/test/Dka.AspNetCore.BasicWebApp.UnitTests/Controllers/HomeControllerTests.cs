using System.Net;
using System.Security.Claims;
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
            var logger = new Mock<ILogger<HomeController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            
            var httpContext = GetHttpContextWithClaims();
            var httpContextAccessor = new HttpContextAccessor {HttpContext = httpContext};
            var homeController = new HomeController(internalApiClient.Object, logger.Object)
            {
                ControllerContext = new ControllerContext {HttpContext = httpContext}
            };

            return (homeController, internalApiClient);
        }
        
        private (HomeController, Mock<IInternalApiClient>) SetupControllerWithThrowingException()
        {
            var logger = new Mock<ILogger<HomeController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            
            var homeController = new HomeController(internalApiClient.Object, logger.Object);

            return (homeController, internalApiClient);
        }        
        
        [Fact]
        public void TestingIndexAction_ShouldPass()
        {
            var (homeController, internalApiClient) = SetupController();

            var result = homeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Home", viewResult.ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi]);
        }
        
        private HttpContext GetHttpContextWithClaims()
        {
            var sampleEmail = "sample-user@test.com";
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, sampleEmail),
                new Claim("email", sampleEmail),
            }, "mock"));

            var sampleHttpContext = new DefaultHttpContext {User = user};
            return sampleHttpContext;
        }
    }
}