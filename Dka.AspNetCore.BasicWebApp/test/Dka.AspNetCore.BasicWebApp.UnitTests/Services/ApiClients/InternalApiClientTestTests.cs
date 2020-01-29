using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.ApiClients
{
    public class InternalApiClientTestTests
    {
        private InternalApiClientTest SetupInternalApiClientTest()
        {
            var httpClient = new Mock<HttpClient>();
            var internalApiClientTest = new InternalApiClientTest(httpClient.Object);

            return internalApiClientTest;
        }

        [Fact]
        public void TestingCreation_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();
            
            Assert.NotNull(internalApiClientTest);
        }

        [Fact]
        public async Task TestingGetTenants_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = (await internalApiClientTest.GetTenants()).ToList();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task TestingGetTenantByGuid_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = await internalApiClientTest.GetTenantByGuid(new Guid("3C54B292-9208-48FE-97A3-6B43A8CC32EF"));

            Assert.Equal("Umbrella Corporation", result.Name);            
        }

        [Fact]
        public void TestingDeleteTenant_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = internalApiClientTest.DeleteTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"));

            Assert.Equal(Task.CompletedTask, result);
        }

        [Fact]
        public async Task TestingCreateNewTenant_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = await internalApiClientTest.CreateNewTenant(new NewTenant
            {
                Name = "Test company 1",
                Alias = "test-company-1"
            });
            
            Assert.Equal(new Guid("C78C30E4-620E-4E2C-8BAF-2A81BA8470A1"), result);
        }

        [Fact]
        public void TestingEditTenant_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = internalApiClientTest.EditTenant(new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), new Tenant
            {
                Name = "Test company 1",
                Alias = "test-company-1",
                Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C")
            });

            Assert.Equal(Task.CompletedTask, result);
        }

        [Fact]
        public async Task TestingCheckApiLiveStatus_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = await internalApiClientTest.CheckApiLiveStatus();
            
            Assert.True(result);
        }
        
        [Fact]
        public async Task TestingCheckApiReadyStatus_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = await internalApiClientTest.CheckApiReadyStatus();
            
            Assert.True(result);
        }
        
        [Fact]
        public async Task TestingCheckApiOverallStatus_ShouldPass()
        {
            var internalApiClientTest = SetupInternalApiClientTest();

            var result = await internalApiClientTest.CheckApiOverallStatus();
            
            Assert.True(result);
        }
    }
}