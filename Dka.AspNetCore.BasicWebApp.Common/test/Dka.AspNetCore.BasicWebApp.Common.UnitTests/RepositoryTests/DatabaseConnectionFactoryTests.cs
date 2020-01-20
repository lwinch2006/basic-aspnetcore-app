using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.RepositoryTests
{
    public class DatabaseConnectionFactoryTests
    {
        [Fact]
        public void TestingDatabaseConnectionFactory_ShouldPass()
        {
            var databaseConfiguration = new DatabaseConfiguration { ConnectionString = "Server=myServerAddress;" };
            var databaseConnectionFactory = new DatabaseConnectionFactory(databaseConfiguration);
            var connection = databaseConnectionFactory.GetConnection();
            
            Assert.NotNull(connection);
            Assert.Equal("Server=myServerAddress;", connection.ConnectionString);
        }
    }
}