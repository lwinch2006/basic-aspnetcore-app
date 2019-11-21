using System;
using Dka.AspNetCore.BasicWebApp.Api.Models.ExceptionProcessing;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Models.ExceptionProcessing
{
    public class ApiDbRunMigrationsExceptionTests
    {
        [Fact]
        public void TestingApiDbRunMigrationsException_PassMessage_ShouldPass()
        {
            var apiDbRunMigrationsException = new ApiDbRunMigrationsException("Hello World!!!");
            Assert.Equal("Hello World!!!", apiDbRunMigrationsException.Message);
        }
        
        [Fact]
        public void TestingApiDbRunMigrationsException_PassMessageAndException_ShouldPass()
        {
            var apiDbRunMigrationsException = new ApiDbRunMigrationsException("Hello World!!!", new ArgumentException());
            Assert.Equal("Hello World!!!", apiDbRunMigrationsException.Message);
            Assert.IsType<ArgumentException>(apiDbRunMigrationsException.InnerException);
        }
    }
}