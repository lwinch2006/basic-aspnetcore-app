using System;
using Dka.AspNetCore.BasicWebApp.ViewModels.Tenants;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.ViewModels.Tenants
{
    public class TenantTests
    {
        [Fact]
        public void TestingTenantViewModel_ShouldPass()
        {
            var tenantVm = new Tenant
            {
                Name = "string-1",
                Alias = "string-2",
                Guid = new Guid("1d121f41-80ed-4ec2-992c-a64040766e68")
            };
            
            Assert.Equal("string-1", tenantVm.Name);
            Assert.Equal("string-2", tenantVm.Alias);
            Assert.Equal(new Guid("1d121f41-80ed-4ec2-992c-a64040766e68"), tenantVm.Guid);

            tenantVm.Name = "string-5";
            tenantVm.Alias = "string-6";
            tenantVm.Guid = new Guid("24f97a9d-d039-447e-ab97-b63fe5d67751");

            Assert.Equal("string-5", tenantVm.Name);
            Assert.Equal("string-6", tenantVm.Alias);
            Assert.Equal(new Guid("24f97a9d-d039-447e-ab97-b63fe5d67751"), tenantVm.Guid);
        }
    }
}