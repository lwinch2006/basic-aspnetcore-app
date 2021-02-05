using System;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.SeleniumTests
{
    public class HomeControllerTests : IClassFixture<BasicWebAppServerFactory<StartupTest>>
    {
        private readonly ChromeOptions _chromeOptions;
        
        private BasicWebAppServerFactory<StartupTest> Server { get; }
        
        private HttpClient Client { get; }

        public HomeControllerTests(
            BasicWebAppServerFactory<StartupTest> server)
        {
            Server = server;
            Client = Server.CreateClient();

            _chromeOptions = new ChromeOptions();
            _chromeOptions.AddArgument("--headless"); 
            _chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
            
            Assert.NotNull(Server);
            Assert.NotNull(Client);
        }
        
        [Fact]
        public void LoadHomePage_CheckPageTitle_ShouldPass()
        {
            using (var browser = new ChromeDriver(".", _chromeOptions))
            {
                var logs = new RemoteLogs(browser); 
                
                Assert.NotNull(browser);
                Assert.NotNull(logs);
                
                browser.Navigate().GoToUrl(Server.RootUri);
            
                Assert.Equal("Sign in", browser.Title);
            
                browser.Close();
            }
        }
        
        [Fact]
        public void LoadHomePage_CheckPageTitle_ShouldNotPass()
        {
            using (var browser = new ChromeDriver(".", _chromeOptions))
            {
                var logs = new RemoteLogs(browser); 
                
                Assert.NotNull(browser);
                Assert.NotNull(logs);
                
                browser.Navigate().GoToUrl(Server.RootUri);
            
                Assert.NotEqual("About", browser.Title);
            
                browser.Close();
            }            
        }        
    }
}