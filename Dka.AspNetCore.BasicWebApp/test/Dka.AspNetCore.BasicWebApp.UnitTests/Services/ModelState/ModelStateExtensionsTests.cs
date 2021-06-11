using System;
using System.Linq;
using System.Security.Claims;
using Dka.AspNetCore.BasicWebApp.Controllers;
using Dka.AspNetCore.BasicWebApp.Services.ModelState;
using Dka.AspNetCore.BasicWebApp.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.ModelState
{
    public class ModelStateExtensionsTests
    {
        private const int SampleInt = 111;
        private const string SampleString = "sample string";
        private readonly HomeController _homeController;

        public ModelStateExtensionsTests()
        {
            _homeController = SetupController();
        }

        [Fact]
        public void ToSummary_NoErrors_ReturnsEmptyString_ShouldPass()
        {
            var modeState = new ModelStateDictionary();

            var result = modeState.ToSummary();

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToSummary_ModelStateIsNull_ReturnsEmptyString_ShouldPass()
        {
            var modeState = new ModelStateDictionary();

            modeState.AddModelError("Test key 1", "Test message 1");
            modeState.AddModelError("Test key 2", "Test message 2");

            var result = modeState.ToSummary();

            Assert.True(result.IndexOf("Test key 1", StringComparison.Ordinal) > -1);
            Assert.True(result.IndexOf("Test message 2", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void ValidateModel_ModelNull_ShouldPass()
        {
            _homeController.ModelState.Clear();
            _homeController.ValidateModel(null, string.Empty);
            var validationSummary = _homeController.ModelState.ToSummary();

            Assert.Contains("Model is null", validationSummary);
        }

        [Theory]
        [InlineData("")]
        [InlineData("null")]
        public void ValidateModel_ModelInvalid_PrefixNullOrEmpty_ShouldPass(string prefix)
        {
            prefix = prefix == "null" ? null : prefix; 
            
            var viewModel = new SignInViewModel
            {
                Username = SampleString,
                Password = string.Empty
            };

            _homeController.ModelState.Clear();
            _homeController.ValidateModel(viewModel, prefix);
            Assert.False(_homeController.ModelState.IsValid);
        }
        
        [Fact]
        public void ValidateModel_ModelInValid_PrefixNotEmpty_ShouldPass()
        {
            var viewModel = new SignInViewModel
            {
                Username = SampleString,
                Password = string.Empty
            };

            _homeController.ModelState.Clear();
            _homeController.ValidateModel(viewModel, "RootProperty.");
            Assert.True(_homeController.ModelState.All(t => t.Key.StartsWith("RootProperty.")));
        }
        
        [Fact]
        public void ValidateModel_ModelValid_ShouldPass()
        {
            var viewModel = new SignInViewModel
            {
                Username = SampleString,
                Password = SampleString
            };

            _homeController.ModelState.Clear();
            _homeController.ValidateModel(viewModel, string.Empty);
            Assert.True(_homeController.ModelState.IsValid);            
        }

        private HomeController SetupController()
        {
            var logger = new Mock<ILogger<HomeController>>();

            var httpContext = GetHttpContextWithClaims();
            var httpContextAccessor = new HttpContextAccessor {HttpContext = httpContext};
            var homeController = new HomeController(null, logger.Object)
            {
                ControllerContext = new ControllerContext {HttpContext = httpContext}
            };

            return homeController;
        }
        
        private HttpContext GetHttpContextWithClaims()
        {
            var sampleEmail = "sample-user@test.com";
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, sampleEmail),
                new Claim("email", sampleEmail),
            }, "mock"));

            var sampleHttpContext = new DefaultHttpContext {User = user};
            return sampleHttpContext;
        }         
    }
}