using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.ModelTests.ApiContractTests
{
    public class NewTenantTests
    {
        [Fact]
        public void TestingNewTenantApiContract_GettingSettingProperties_ShouldPass()
        {
            var newTenantApiContract = new NewTenant
            {
                Name = "Test tenant",
                Alias = "test-tenant"
            };

            Assert.Equal("Test tenant", newTenantApiContract.Name);
            Assert.Equal("test-tenant", newTenantApiContract.Alias);

            newTenantApiContract.Name = "Test tenant 111";
            Assert.Equal("Test tenant 111", newTenantApiContract.Name);
            
            newTenantApiContract.Alias = "test-tenant-111";
            Assert.Equal("test-tenant-111", newTenantApiContract.Alias);
        }
    }
}