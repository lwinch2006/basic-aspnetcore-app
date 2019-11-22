using System;
using Dka.AspNetCore.BasicWebApp.Models.Tenants;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Models.Tenants
{
    public class TenantNotFoundExceptionTests
    {
        [Fact]
        public void TestingTenantNotFoundException_ShouldPass()
        {
            var tenantNotFoundException = new TenantNotFoundException("Hello World!!!");
            Assert.Equal("Hello World!!!", tenantNotFoundException.Message);
            
            tenantNotFoundException = new TenantNotFoundException("Hello World???", new FormatException());
            Assert.Equal("Hello World???", tenantNotFoundException.Message);
            Assert.IsType<FormatException>(tenantNotFoundException.InnerException);
            
            tenantNotFoundException = new TenantNotFoundException(new InvalidOperationException());
            Assert.IsType<InvalidOperationException>(tenantNotFoundException.InnerException);
            
            tenantNotFoundException = new TenantNotFoundException();
            Assert.Equal("Tenant not found exception", tenantNotFoundException.Message);
        }
    }
}