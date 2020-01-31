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
    public class StatusControllerTests
    {
        private (StatusController, Mock<IInternalApiClient>) SetupController()
        {
            var logger = new Mock<ILogger<StatusController>>();
            var internalApiClient = new Mock<IInternalApiClient>();
            internalApiClient.Setup(client => client.CheckApiLiveStatus()).Returns(() => Task.FromResult(true)).Verifiable();
            
            var statusController = new StatusController(internalApiClient.Object, logger.Object);

            return (statusController, internalApiClient);
        }
        
        [Fact]
        public async Task TestingIndexAction_ShouldPass()
        {
            var (statusController, internalApiClient) = SetupController();

            var result = await statusController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.True((bool)viewResult.ViewData[ViewDataKeys.ApiLiveStatus]);
            internalApiClient.Verify();            
        }
    }
}