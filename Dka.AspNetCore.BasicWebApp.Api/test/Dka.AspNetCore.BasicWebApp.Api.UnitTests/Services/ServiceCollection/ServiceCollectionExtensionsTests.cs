using Dka.AspNetCore.BasicWebApp.Api.Services.ServiceCollection;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Api.UnitTests.Services.ServiceCollection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void TestingServiceCollectionExtensions_ShouldPass()
        {
            var databaseConfiguration = new DatabaseConfiguration {ConnectionString = "Hello World!!!"};
            var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var tenantLogic =  serviceProvider.GetService<TenantLogic>();
            Assert.Null(tenantLogic);
            
            serviceCollection.AddDatabaseClasses(databaseConfiguration);
            serviceProvider = serviceCollection.BuildServiceProvider();
            tenantLogic =  serviceProvider.GetService<TenantLogic>();
            Assert.NotNull(tenantLogic);
        }
    }
}