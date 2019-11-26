using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.SystemTests
{
    public class HomeControllerTests : IClassFixture<BasicWebAppServerFactory<Startup>>, IClassFixture<BasicWebAppServerFactory<Api.Startup>>
    {
        private BasicWebAppServerFactory<Startup> WebServer { get; }
        
        private HttpClient WebClient { get; }        
        
        private BasicWebAppServerFactory<Api.Startup> ApiServer { get; }
        
        private HttpClient ApiClient { get; }
        
        public HomeControllerTests(
            BasicWebAppServerFactory<Startup> webServer, BasicWebAppServerFactory<Api.Startup> apiServer)
        {
            WebServer = webServer;
            WebClient = WebServer.CreateClient();

            ApiServer = apiServer;
            ApiClient = ApiServer.CreateClient();
            
            
            Assert.NotNull(WebServer);
            Assert.NotNull(WebClient);
            Assert.NotNull(ApiServer);
            Assert.NotNull(ApiClient);
        }        
        
        [Fact]
        public async Task LoadHomePage_CheckContentType_ShouldPass()
        {
            var response = await WebClient.GetAsync(WebServer.RootUri);

            response.EnsureSuccessStatusCode();
            
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
        
        [Theory]
        [InlineData("/Home")]
        [InlineData("/About")]
        public async Task LoadHomePage_CheckPageTitle_ShouldPass(string url)
        {
            var expectedPageName = url.Replace("/", string.Empty);
            
            var response = await WebClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode();
            
            var contentFromResponse = await response.Content.ReadAsStringAsync();
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(contentFromResponse));
            
            Assert.Equal(expectedPageName, document.Head.QuerySelector("title").Text());
        }
    }
}