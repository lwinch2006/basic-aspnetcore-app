using System;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.ApiClients
{
    public class ApiConnectionExceptionTests
    {
        [Fact]
        public void TestingApiConnectionException_ShouldPass()
        {
            var apiConnectionException = new ApiConnectionException("Hello World!!!");
            Assert.Equal("Hello World!!!", apiConnectionException.Message);
            
            apiConnectionException = new ApiConnectionException("Hello World???", new FormatException());
            Assert.Equal("Hello World???", apiConnectionException.Message);
            Assert.IsType<FormatException>(apiConnectionException.InnerException);
            
            apiConnectionException = new ApiConnectionException(new InvalidOperationException());
            Assert.IsType<InvalidOperationException>(apiConnectionException.InnerException);
        }
    }
}