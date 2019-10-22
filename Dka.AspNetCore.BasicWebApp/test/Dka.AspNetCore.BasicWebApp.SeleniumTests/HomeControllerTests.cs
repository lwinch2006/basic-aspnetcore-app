using System;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.SeleniumTests
{
    public class HomeControllerTests : IClassFixture<BasicWebAppSeleniumServerFactory<Startup>>
    {
        private BasicWebAppSeleniumServerFactory<Startup> Server { get; }
        
        private IWebDriver Browser { get; }
        
        private HttpClient Client { get; }
        
        private ILogs Logs { get; }

        public HomeControllerTests(
            BasicWebAppSeleniumServerFactory<Startup> server)
        {
            Server = server;
            Client = Server.CreateClient();

            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArgument("--headless"); 
            chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);

            var driver = new RemoteWebDriver(chromeOptions);
            Browser = driver;
            Logs = new RemoteLogs(driver);
        }
        
        [Fact]
        public void LoadHomePage_ShouldPass()
        {
            Browser.Navigate().GoToUrl(Server.RootUri);
            Assert.StartsWith("Home", Browser.Title);
        }
    }
}