using Dka.AspNetCore.BasicWebApp.ViewModels.Tenants;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.ViewModels.Tenants
{
    public class NewTenantTests
    {
        [Fact]
        public void TestingNewTenant_ShouldPass()
        {
            var newTenantVm = new NewTenantViewModel
            {
                Name = "string-1",
                Alias = "string-2"
            };
            
            Assert.Equal("string-1", newTenantVm.Name);
            Assert.Equal("string-2", newTenantVm.Alias);

            newTenantVm.Name = "string-5";
            newTenantVm.Alias = "string-6";
            
            Assert.Equal("string-5", newTenantVm.Name);
            Assert.Equal("string-6", newTenantVm.Alias);
        }
    }
}