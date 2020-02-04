using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.ModelTests.ApiContractTests
{
    public class TenantTests
    {
        [Fact]
        public void TestingTenantApiContract_GettingSettingProperties_ShouldPass()
        {
            var tenant = new TenantContract
            {
                Name = "Test tenant",
                Alias = "test-tenant",
                Guid = new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6")
            };

            Assert.Equal("Test tenant", tenant.Name);
            Assert.Equal("test-tenant", tenant.Alias);
            Assert.Equal(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6"), tenant.Guid);

            tenant.Name = "Test tenant 111";
            Assert.Equal("Test tenant 111", tenant.Name);

            tenant.Alias = "test-tenant-111";
            Assert.Equal("test-tenant-111", tenant.Alias);

            tenant.Guid = new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF");
            Assert.Equal(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), tenant.Guid);
        }
    }
}