using System;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.ApiClients
{
    public class ApiStatusCodeExceptionTests
    {
        [Fact]
        public void TestingApiStatusCodeException_ShouldPass()
        {
            var apiStatusCodeException = new ApiStatusCodeException("Hello World!!!");
            Assert.Equal("Hello World!!!", apiStatusCodeException.Message);
            
            apiStatusCodeException = new ApiStatusCodeException("Hello World???", new FormatException());
            Assert.Equal("Hello World???", apiStatusCodeException.Message);
            Assert.IsType<FormatException>(apiStatusCodeException.InnerException);
            
            apiStatusCodeException = new ApiStatusCodeException(new InvalidOperationException());
            Assert.IsType<InvalidOperationException>(apiStatusCodeException.InnerException);           
        }
    }
}