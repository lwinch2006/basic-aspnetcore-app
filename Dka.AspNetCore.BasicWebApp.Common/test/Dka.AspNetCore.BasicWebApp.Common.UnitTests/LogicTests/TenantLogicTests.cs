using System;
using System.Linq;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests
{
    public class TenantLogicTests
    {
        private TenantLogic SetupTenantLogic()
        {
            var databaseConfiguration = new Mock<DatabaseConfiguration>();
            var databaseConnectionFactory = new Mock<DatabaseConnectionFactory>(databaseConfiguration.Object);
            var tenantRepository = new Mock<TenantRepository>(databaseConnectionFactory.Object);
            
            tenantRepository.Setup(repository => repository.Get(It.IsAny<Pagination>())).Returns<Pagination>(
                pagination =>
                {
                    var tenants = Tenant.GetDummyTenantSet().GetAwaiter().GetResult();
                    var result = new PagedResults<Tenant>
                    {
                        Items = tenants,
                        TotalCount = tenants.Count()
                    };

                    return Task.FromResult(result);
                });
        
            tenantRepository.Setup(repository => repository.Get(It.IsAny<Guid>())).Returns<Guid>(guid =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                return Task.FromResult(tenantList.FirstOrDefault(record => record.Guid == guid));
            });
            
            tenantRepository.Setup(repository => repository.Create(It.IsAny<Tenant>())).Returns<Tenant>(tenant =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();
                tenant.Guid = new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6");
                tenant.Id = tenantList.Max(record => record.Id) + 1;
                tenantList.Add(tenant);

                return Task.FromResult(tenantList[^1].Guid);
            });
            
            tenantRepository.Setup(repository => repository.Update(It.IsAny<Tenant>())).Returns<Tenant>(tenant =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                if (tenantList.Find(record => record.Guid == tenant.Guid) is { } foundTenant)
                {
                    foundTenant = tenant;
                    return Task.FromResult(1);
                }
                
                return Task.FromResult(0);
            });
            
            tenantRepository.Setup(repository => repository.Delete(It.IsAny<Guid>())).Returns<Guid>(guid =>
            {
                var tenantList = Tenant.GetDummyTenantSet().GetAwaiter().GetResult().ToList();

                if (tenantList.Find(record => record.Guid == guid) is { } foundTenant)
                {
                    tenantList.Remove(foundTenant);
                    return Task.FromResult(1);
                }

                return Task.FromResult(0);
            });            
            
            var tenantLogic = new TenantLogic(tenantRepository.Object);

            return tenantLogic;
        }
        
        [Fact]
        public async Task GetAll_ReturnsAllTenants_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();

            var tenants = (await tenantLogic.Get()).Items.ToList();

            Assert.Equal(3, tenants.Count);
            Assert.Equal("Umbrella Corporation", tenants.First().Name);
        }

        [Fact]
        public async Task GetByGuid_PassValidGuid_ReturnsTenant_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            var tenant = await tenantLogic.Get(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            Assert.NotNull(tenant);
            Assert.Equal("Umbrella Corporation", tenant.Name);
        }
        
        [Fact]
        public async Task GetByGuid_PassInvalidGuid_ReturnsTenant_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            var tenant = await tenantLogic.Get(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));

            Assert.Null(tenant);
        }

        [Fact]
        public async Task CreateNewTenant_PassValidTenant_ReturnsNewTenantGuid_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            var tenantGuid = await tenantLogic.Create(new Tenant {Name = "Test company", Alias = "test-company"});
            
            Assert.Equal(new Guid("DE5BC94F-80E7-44AB-B1EF-BDFF7E47C2D6"), tenantGuid);
        }

        [Fact]
        public async Task CreateNewTenant_PassNullTenant_ReturnsException_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();

            try
            {
                await tenantLogic.Create(null);
            }
            catch (NullReferenceException)
            {
                Assert.True(true, "NullReferenceException has happened. This is correct behaviour.");
                return;
            }
            
            Assert.True(false, "NullReferenceException has not happened. This is incorrect behaviour.");
        }

        [Fact]
        public async Task EditTenant_PassValidTenant_ReturnsNewTenantGuid_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            var affectedRows = await tenantLogic.Update(new Tenant {Name = "Test company", Alias = "test-company", Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")});
            
            Assert.Equal(1, affectedRows);            
        }
        
        [Fact]
        public async Task EditTenant_PassInvalidTenant_ReturnsNewTenantGuid_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            var affectedRows = await tenantLogic.Update(new Tenant {Name = "Test company", Alias = "test-company", Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")});
            
            Assert.Equal(0, affectedRows);            
        }
        
        [Fact]
        public async Task EditTenant_PassNullTenant_ReturnsException_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();

            try
            {
                await tenantLogic.Update(null);
            }
            catch (NullReferenceException)
            {
                Assert.True(true, "NullReferenceException has happened. This is correct behaviour.");
                return;
            }
            
            Assert.True(false, "NullReferenceException has not happened. This is incorrect behaviour.");
        }

        [Fact]
        public async Task DeleteTenant_PassValidGuid_ReturnsVoid_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            
            var affectedRows = await tenantLogic.Delete(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            
            Assert.Equal(1, affectedRows);
        }
        
        [Fact]
        public async Task DeleteTenant_PassInvalidGuid_ReturnsVoid_ShouldPass()
        {
            var tenantLogic = SetupTenantLogic();
            
            var affectedRows = await tenantLogic.Delete(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));
            
            Assert.Equal(0, affectedRows);
        }        
    }
}