using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.IntegrationTests
{
    public class HomeControllerTests : IClassFixture<BasicWebAppServerFactory<StartupTest>>
    {
        private BasicWebAppServerFactory<StartupTest> Server { get; }
        
        private HttpClient Client { get; }
        
        public HomeControllerTests(
            BasicWebAppServerFactory<StartupTest> server)
        {
            Server = server;
            Client = Server.CreateClient();
        }
        
        [Fact]
        public async Task LoadHomePage_CheckContentType_ShouldPass()
        {
            var response = await Client.GetAsync(Server.RootUri);

            response.EnsureSuccessStatusCode();
            
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task LoadHomePage_CheckPageTitle_ShouldPass()
        {
            var response = await Client.GetAsync(Server.RootUri);
            
            response.EnsureSuccessStatusCode();
            
            var contentFromResponse = await response.Content.ReadAsStringAsync();
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(contentFromResponse));
            
            Assert.Equal("Sign in", document.Head.QuerySelector("title").Text());
        }
    }
}