using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Controllers
{
    public class PageControllerTests
    {
        [Fact]
        public async Task GetPageName_PassValidPageName_ReturnsPageName_ShouldPass()
        {
            var pageController = new PagesController();

            var result = await pageController.GetPageName("Hello World!!!");
            
            var viewResult = Assert.IsType<OkObjectResult>(result);
            
            Assert.Equal("Hello World!!!", viewResult.Value.ToString());
        }
    }
}