using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dapper;

namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public class TenantRepository
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public TenantRepository(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        internal async Task<IEnumerable<Tenant>> GetAll()
        {
            await using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                const string query = @"
                    SELECT [Id], [Name], [Alias], [ExternalId]
                    FROM [Tenants]
                ";

                var tenants = await connection.QueryAsync<Tenant>(query);

                return tenants;
            }
        }
    }
}