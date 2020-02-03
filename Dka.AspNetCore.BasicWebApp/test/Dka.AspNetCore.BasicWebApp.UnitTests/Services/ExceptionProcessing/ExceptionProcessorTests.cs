using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Common.Models.Toastr;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.ExceptionProcessing
{
    public class ExceptionProcessorTests
    {
        [Fact]
        public void TestingApiConnectionException_ShouldPass()
        {
            var logger = new Mock<ILogger>();
            var httpContext = new DefaultHttpContext();
            var apiConnectionException = new ApiConnectionException("Hello World!!!");
            
            ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, logger.Object, httpContext, apiConnectionException);
            
            Assert.Equal(UserFriendlyErrorMessageConstants.ApiConnectionException, httpContext.Items[ToastrConstants.Message]);
        }
        
        [Fact]
        public void TestingApiStatusCodeException_ShouldPass()
        {
            var logger = new Mock<ILogger>();
            var httpContext = new DefaultHttpContext();
            var apiStatusCodeException = new ApiStatusCodeException("Hello World!!!");
            
            ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, logger.Object, httpContext, apiStatusCodeException);
            
            Assert.Equal(UserFriendlyErrorMessageConstants.ApiStatusCodeException, httpContext.Items[ToastrConstants.Message]);
        }        
        
        [Fact]
        public void TestingBasicWebAppException_ShouldPass()
        {
            var logger = new Mock<ILogger>();
            var httpContext = new DefaultHttpContext();
            var basicWebAppException = new BasicWebAppException("Hello World!!!");
            
            ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, logger.Object, httpContext, basicWebAppException);
            
            Assert.Equal(UserFriendlyErrorMessageConstants.GeneralException, httpContext.Items[ToastrConstants.Message]);
        } 
    }
}