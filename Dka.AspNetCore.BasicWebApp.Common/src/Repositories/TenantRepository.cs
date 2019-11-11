using System;
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
                    SELECT [Id], [Name], [Alias], [Guid]
                    FROM [Tenants]
                ";

                var tenants = await connection.QueryAsync<Tenant>(query);

                return tenants;
            }
        }

        internal async Task<Tenant> GetByGuid(Guid guid)
        {
            await using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                const string query = @"
                    SELECT [Id], [Name], [Alias], [Guid]
                    FROM [Tenants]
                    WHERE [Guid] = @Guid
                ";

                var tenant = await connection.QuerySingleOrDefaultAsync<Tenant>(query, new { @Guid = guid });

                return tenant;
            }
        }

        internal async Task<Guid> CreateNewTenant(Tenant newTenantBo)
        {
            await using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                newTenantBo.Guid = Guid.NewGuid();
                
                const string query = @"
                    INSERT INTO [Tenants] ([Alias], [Name], [Guid])
                    VALUES (@Alias, @Name, @Guid);
                ";

                await connection.ExecuteAsync(query,
                    new {@Alias = newTenantBo.Alias, @Name = newTenantBo.Name, @Guid = newTenantBo.Guid});

                return newTenantBo.Guid;
            }
        }

        internal async Task EditTenant(Tenant tenantToEdit)
        {
            await using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                const string query = @"
                    UPDATE [Tenants]
                    SET 
                        [Alias] = @Alias,
                        [Name] = @Name
                    WHERE
                        [Guid] = @Guid;
                ";

                await connection.ExecuteAsync(query,
                    new {@Alias = tenantToEdit.Alias, @Name = tenantToEdit.Name, @Guid = tenantToEdit.Guid});
            }
        }

        internal async Task DeleteTenant(Guid guid)
        {
            await using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                const string query = @"
                    DELETE FROM [Tenants]
                    WHERE [Guid] = @Guid;
                ";
                
                await connection.ExecuteAsync(query, new {@Guid = guid});
            }
        }
    }
}