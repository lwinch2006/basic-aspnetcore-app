using System.Data;
using System.Data.SqlClient;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;

namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public class DatabaseConnectionFactory
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public DatabaseConnectionFactory(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public virtual IDbConnection GetConnection()
        {
            return new SqlConnection(_databaseConfiguration.ConnectionString);
        }
    }
}