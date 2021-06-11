using System.IO;
using System.Security.Claims;
using System.Text;
using Dka.AspNetCore.BasicWebApp.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Utils
{
    public class SignInViewModelBinderTests
    {
        [Fact]
        public void Test_BindingContextNull_ThrowsException_ShouldPass()
        {
            var exception = Record.Exception(() =>
                new SignInViewModelBinder().BindModelAsync(null).GetAwaiter().GetResult());
            
            Assert.NotNull(exception);
        }

        [Theory]
        [InlineData("        ")]
        [InlineData("")]
        public void Test_JsonStringInvalid_ReturnsFailedResult_ShouldPass(string jsonAsString)
        {
            var modelBinder = new SignInViewModelBinder();
            var modelBinderContext = SetupBindingContext(jsonAsString);

            var exception =
                Record.Exception(() => modelBinder.BindModelAsync(modelBinderContext).GetAwaiter().GetResult());

            Assert.Null(exception);
            Assert.Equal(ModelBindingResult.Failed(), modelBinderContext.Result);
        }        
        
        [Theory]
        [InlineData("\"\"")]
        public void Test_JsonStringInvalid_ThrowsException_ShouldPass(string jsonAsString)
        {
            var modelBinder = new SignInViewModelBinder();
            var modelBinderContext = SetupBindingContext(jsonAsString);

            var exception =
                Record.Exception(() => modelBinder.BindModelAsync(modelBinderContext).GetAwaiter().GetResult());

            Assert.NotNull(exception);
        }

        [Theory]
        [InlineData("{}")]
        [InlineData(@"{""username"": ""123"", ""path"": {""to"": {""password"": ""Test test test""}}}")]
        public void Test_JsonStringValid_ReturnsSuccessResult_ShouldPass(string jsonAsString)
        {
            var modelBinder = new SignInViewModelBinder();
            var modelBinderContext = SetupBindingContext(jsonAsString);

            var exception =
                Record.Exception(() => modelBinder.BindModelAsync(modelBinderContext).GetAwaiter().GetResult());

            Assert.Null(exception);
            Assert.True(modelBinderContext.Result.IsModelSet);
        }        
        
        private ModelBindingContext SetupBindingContext(string requestBody)
        {
            var httpContext = GetHttpContextWithClaims();
            
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = stream.Length;

            var actionContext = new ActionContext
            {
                HttpContext = httpContext
            };
            
            var modelBinderContext = new DefaultModelBindingContext
            {
                ActionContext = actionContext
            };

            return modelBinderContext;
        }

        private HttpContext GetHttpContextWithClaims()
        {
            const string sampleEmail = "sample-user-001@test.com";
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, sampleEmail),
                new Claim("email", sampleEmail)
            }, "mock"));

            var sampleHttpContext = new DefaultHttpContext {User = user};
            return sampleHttpContext;
        }        
    }
}