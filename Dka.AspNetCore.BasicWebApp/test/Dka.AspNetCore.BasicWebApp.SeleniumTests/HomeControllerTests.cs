using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.SeleniumTests
{
    public class HomeControllerTests : IClassFixture<BasicWebAppServerFactory<StartupTest>>
    {
        private BasicWebAppServerFactory<StartupTest> Server { get; }
        
        private IWebDriver Browser { get; }
        
        private HttpClient Client { get; }
        
        private ILogs Logs { get; }

        public HomeControllerTests(
            BasicWebAppServerFactory<StartupTest> server)
        {
            Server = server;
            Client = Server.CreateClient();

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless"); 
            chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);

            var driver = new RemoteWebDriver(chromeOptions);
            Browser = driver;
            Logs = new RemoteLogs(driver);
            
            Assert.NotNull(Server);
            Assert.NotNull(Client);
            Assert.NotNull(Browser);
            Assert.NotNull(Logs);
        }
        
        [Fact]
        public void LoadHomePage_CheckPageTitle_ShouldPass()
        {
            Browser.Navigate().GoToUrl(Server.RootUri);
            
            Assert.Equal("Home", Browser.Title);
            
            Browser.Close();
        }
        
        [Fact]
        public void LoadHomePage_CheckPageTitle_ShouldNotPass()
        {
            Browser.Navigate().GoToUrl(Server.RootUri);
            
            Assert.NotEqual("About", Browser.Title);
            
            Browser.Close();
        }        
    }
}