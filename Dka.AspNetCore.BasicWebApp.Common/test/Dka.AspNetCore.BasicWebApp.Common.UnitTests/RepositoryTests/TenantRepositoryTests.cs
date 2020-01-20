using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.RepositoryTests
{
    public class TenantRepositoryTests
    {
        private async Task<TenantRepository> SetupTenantRepository()
        {
            var inMemoryDatabase = new TestSQLiteInMemoryDatabase().GetConnection();
            var tenants = (await Tenant.GetDummyTenantSet()).ToList();
            
            const string createTenantsTableQuery = @"
                CREATE TABLE Tenants (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Alias TEXT NOT NULL,
                    Guid TEXT NOT NULL UNIQUE
                );                
            ";

            await inMemoryDatabase.ExecuteAsync(createTenantsTableQuery);
            
            const string insertTenantQuery = @"
                INSERT INTO Tenants (Id, Name, Alias, Guid)
                VALUES (@Id, @Name, @Alias, @Guid);
            ";

            foreach (var tenant in tenants)
            {
                await inMemoryDatabase.ExecuteAsync(insertTenantQuery, new
                {
                    @Id = tenant.Id, 
                    @Name = tenant.Name,
                    @Alias = tenant.Alias,
                    @Guid = tenant.Guid.ToString()
                });
            }
            
            var databaseConfiguration = new Mock<DatabaseConfiguration>();
            var databaseConnectionFactory = new Mock<DatabaseConnectionFactory>(databaseConfiguration.Object);
            databaseConnectionFactory.Setup(factory => factory.GetConnection()).Returns(inMemoryDatabase);

            var tenantRepository = new TenantRepository(databaseConnectionFactory.Object); 
            SqlMapper.AddTypeHandler(typeof(Guid), new TestGuidTypeHandler());

            return tenantRepository;
        }

        [Fact]
        public async Task GetAll_ShouldPass()
        {
            var tenants = (await Tenant.GetDummyTenantSet()).ToList();
            var tenantRepository = await SetupTenantRepository();
            
            var result = (await tenantRepository.GetAll()).ToList();
            Assert.Equal(tenants.Count, result.Count);
            Assert.True(tenants.All(record1 => result.SingleOrDefault(record2 => record2.Id == record1.Id && record2.Name == record1.Name && record2.Alias == record1.Alias && record2.Guid == record1.Guid) != null));

        }

        [Fact]
        public async Task GetByGuid_PassValidGuid_ReturnsTenant_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var foundTenant = await tenantRepository.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            
            Assert.Equal("Umbrella Corporation", foundTenant.Name);
            Assert.Equal("umbrella", foundTenant.Alias);
        }

        [Fact]
        public async Task GetByGuid_PassInvalidGuid_ReturnsNull_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var foundTenant = await tenantRepository.GetByGuid(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));
            
            Assert.Null(foundTenant);
        }

        [Fact]
        public async Task CreateNewTenant_PassValidTenant_ReturnsGuid_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var newTenantGuid = await tenantRepository.CreateNewTenant(new Tenant
            {
                Name = "Test company",
                Alias = "test-company"
            });

            Assert.True(newTenantGuid != Guid.Empty);
        }

        [Fact]
        public async Task CreateNewTenant_PassInvalidTenant_ReturnsGuid_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();

            try
            {
                await tenantRepository.CreateNewTenant(null);
            }
            catch (NullReferenceException)
            {
                Assert.True(true, "NullReferenceException has happened. This is correct behaviour.");
                return;
            }
            
            Assert.True(false, "NullReferenceException has not happened. This is incorrect behaviour.");
        }        

        [Fact]
        public async Task DeleteTenant_PassValidGuid_TenantListReduced_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var affectedRows = await tenantRepository.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));
            
            Assert.Equal(1, affectedRows);
        }
        
        [Fact]
        public async Task DeleteTenant_PassInvalidGuid_TenantListNotChanged_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var affectedRows = await tenantRepository.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF"));
            
            Assert.Equal(0, affectedRows);
        }

        [Fact]
        public async Task EditTenant_PassValidTenant_TenantIsChanged_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var affectedRows = await tenantRepository.EditTenant(new Tenant
            {
                Name = "Umbrella Corporation 111",
                Alias = "umbrella-111",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            Assert.Equal(1, affectedRows);
        }
        
        [Fact]
        public async Task EditTenant_PassInvalidTenant_NoTenantIsChanged_ShouldPass()
        {
            var tenantRepository = await SetupTenantRepository();
            var affectedRows = await tenantRepository.EditTenant(new Tenant
            {
                Name = "Umbrella Corporation 111",
                Alias = "umbrella-111",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0BFFF")
            });

            Assert.Equal(0, affectedRows);
        }
    }
}