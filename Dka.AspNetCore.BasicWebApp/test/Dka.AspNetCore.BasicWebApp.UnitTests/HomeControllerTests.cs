using System.Net;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Controllers;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_ShouldPass()
        {
            var mockedInternalApiClient = new Mock<IInternalApiClient>();
            mockedInternalApiClient.Setup(client => client.GetPageNameAsync("Home")).ReturnsAsync("Home").Verifiable();

            var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();
            
            var mockedLogger = new Mock<ILogger<HomeController>>();
            
            var homeController = new HomeController(mockedInternalApiClient.Object, mockedHttpContextAccessor.Object, mockedLogger.Object);

            var result = await homeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal("Home", viewResult.ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi]);
            mockedInternalApiClient.Verify();
        }
    }
}