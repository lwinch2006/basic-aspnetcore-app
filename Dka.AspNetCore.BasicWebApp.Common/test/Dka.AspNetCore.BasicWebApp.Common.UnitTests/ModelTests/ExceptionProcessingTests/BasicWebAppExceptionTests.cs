using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.ModelTests.ExceptionProcessingTests
{
    public class BasicWebAppExceptionTests
    {
        [Fact]
        public void TestingBasicWebAppExceptionModel_GettingSettingProperties_ShouldPass()
        {
            var basicWebAppException = new BasicWebAppException();
            Assert.Equal("Basic exception", basicWebAppException.Message);
            
            basicWebAppException = new BasicWebAppException("New exception message");
            Assert.Equal("New exception message", basicWebAppException.Message);
            
            basicWebAppException = new BasicWebAppException(new ArgumentException());
            Assert.IsType<ArgumentException>(basicWebAppException.InnerException);
            
            basicWebAppException = new BasicWebAppException("Again new exception message", new IndexOutOfRangeException());
            Assert.Equal("Again new exception message", basicWebAppException.Message);
            Assert.IsType<IndexOutOfRangeException>(basicWebAppException.InnerException);
        }
    }
}