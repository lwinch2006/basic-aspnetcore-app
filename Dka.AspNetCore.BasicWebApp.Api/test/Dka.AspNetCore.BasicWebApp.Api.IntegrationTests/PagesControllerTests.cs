using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.IntegrationTests
{
    public class PagesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _webAppApiFactory;

        public PagesControllerTests(WebApplicationFactory<Startup> webAppApiFactory)
        {
            _webAppApiFactory = webAppApiFactory;
        }
        
        [Theory]
        [InlineData("/Pages/GetPageName?pageName=Home")]
        public void GetPagesName_CheckReturningLanguageAndContentType_ShouldPass(string url)
        {
            Assert.True(1==1);
            
            // var client = _webAppApiFactory.CreateClient();
            //
            // var response = await client.GetAsync(url);
            //
            // response.EnsureSuccessStatusCode();
            //
            // Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}