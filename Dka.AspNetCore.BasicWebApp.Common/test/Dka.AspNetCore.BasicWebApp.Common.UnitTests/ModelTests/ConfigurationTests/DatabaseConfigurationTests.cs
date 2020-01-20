using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.ModelTests.ConfigurationTests
{
    public class DatabaseConfigurationTests
    {
        [Fact]
        public void TestingDatabaseConfigurationModel_GettingSettingProperties_ShouldPass()
        {
            var databaseConfiguration = new DatabaseConfiguration
            {
                ConnectionString = "Test connection string"
            };
            
            Assert.Equal("Test connection string", databaseConfiguration.ConnectionString);

            databaseConfiguration.ConnectionString = "New test connection string";
            Assert.Equal("New test connection string", databaseConfiguration.ConnectionString);
        }
    }
}