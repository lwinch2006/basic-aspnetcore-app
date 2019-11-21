using Dka.AspNetCore.BasicWebApp.Api.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Services.ExceptionProcessing
{
    public class ExceptionProcessorTests
    {
        [Fact]
        public void TestingExceptionProcessor_ShouldPass()
        {
            var logger = new Mock<ILogger>();
            var basicWebAppException = new BasicWebAppException();

            ExceptionProcessor.Process(logger.Object, basicWebAppException);

            Assert.True(true, "Testing completed without exceptions. This behaviour should be.");
        }
    }
}