using System.Threading.Tasks;
using AutoMapper;
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
    public class AdministrationControllerTests
    {
        private AdministrationController SetupController()
        {
            var logger = new Mock<ILogger<AdministrationController>>();
            var mapper = new Mock<IMapper>();
            var internalApiClient = new Mock<IInternalApiClient>();
            var administrationController = new AdministrationController(internalApiClient.Object, logger.Object, mapper.Object);

            return administrationController;
        }
        
        [Fact]
        public void TestingIndexAction_ShouldPass()
        {
            var administrationController = SetupController();

            var result = administrationController.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}