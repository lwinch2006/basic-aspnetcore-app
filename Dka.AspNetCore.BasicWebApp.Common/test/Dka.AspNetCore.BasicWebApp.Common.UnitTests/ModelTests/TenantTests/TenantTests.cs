using System;
using System.Linq;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.ModelTests.TenantTests
{
    public class TenantTests
    {
        [Fact]
        public void TestingTenantModel_GettingSettingProperties_ShouldPass()
        {
            var tenant = new Tenant
            {
                Id = 111,
                Name = "Test tenant",
                Alias = "test-tenant",
                Guid = new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6")
            };

            
            Assert.Equal(111, tenant.Id);
            Assert.Equal("Test tenant", tenant.Name);
            Assert.Equal("test-tenant", tenant.Alias);
            Assert.Equal(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6"), tenant.Guid);

            tenant.Id = 222;
            Assert.Equal(222, tenant.Id);

            tenant.Name = "Test tenant 222";
            Assert.Equal("Test tenant 222", tenant.Name);

            tenant.Alias = "test-tenant-222";
            Assert.Equal("test-tenant-222", tenant.Alias);

            tenant.Guid = new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF");
            Assert.Equal(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47CFFF"), tenant.Guid);
        }

        [Fact]
        public async Task TestingTenantModel_GettingDummyTenantSet_ShouldPass()
        {
            var tenants = (await Tenant.GetDummyTenantSet()).ToList();
            
            Assert.Equal(3, tenants.Count);
            Assert.Equal("Umbrella Corporation", tenants[0].Name);
        }
    }
}