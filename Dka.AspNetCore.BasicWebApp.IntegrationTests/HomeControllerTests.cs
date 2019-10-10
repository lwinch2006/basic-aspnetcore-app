using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dka.AspNetCore.BasicWebApp.IntegrationTests
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>, IClassFixture<WebApplicationFactory<Api.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _webAppFactory;
        private readonly WebApplicationFactory<Api.Startup> _webAppApiFactory;

        public HomeControllerTests(WebApplicationFactory<Startup> webAppFactory, WebApplicationFactory<Api.Startup> webAppApiFactory)
        {
            _webAppFactory = webAppFactory;
            _webAppApiFactory = webAppApiFactory;
            
            _webAppFactory.Server.BaseAddress = new Uri("https://localhost:5556");
            _webAppApiFactory.Server.BaseAddress = new Uri("https://localhost:6556");
        }
        
        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        public async Task Index_CheckEndPointsAndReturnedContentType_ShouldPass(string url)
        {
            var webAppApiHttpClient = _webAppApiFactory.CreateClient();
            var webAppHttpClient = _webAppFactory.CreateClient();
            
            
            
            
            

            var response = await webAppHttpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}