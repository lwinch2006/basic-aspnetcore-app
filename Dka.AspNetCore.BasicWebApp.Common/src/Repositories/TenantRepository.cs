using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dapper;

[assembly: InternalsVisibleTo("Dka.AspNetCore.BasicWebApp.Common.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public class TenantRepository
    {
        private readonly DatabaseConnectionFactory _databaseConnectionFactory;

        public TenantRepository(DatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        internal virtual async Task<IEnumerable<Tenant>> GetAll()
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Id], [Name], [Alias], [Guid]
                    FROM [Tenants]
                ";

                var tenants = await connection.QueryAsync<Tenant>(query);

                return tenants;
            }
        }

        internal virtual async Task<Tenant> GetByGuid(Guid guid)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT Id, Name, Alias, Guid
                    FROM Tenants
                    WHERE Guid = @Guid
                ";

                var tenant = await connection.QuerySingleOrDefaultAsync<Tenant>(query, new { @Guid = guid.ToString() });

                return tenant;
            }
        }

        internal virtual async Task<Guid> CreateNewTenant(Tenant newTenantBo)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                newTenantBo.Guid = Guid.NewGuid();
                
                const string query = @"
                    INSERT INTO [Tenants] ([Alias], [Name], [Guid])
                    VALUES (@Alias, @Name, @Guid);
                ";

                var affectedRows = await connection.ExecuteAsync(query,
                    new {@Alias = newTenantBo.Alias, @Name = newTenantBo.Name, @Guid = newTenantBo.Guid.ToString()});

                return affectedRows == 1 ? newTenantBo.Guid : Guid.Empty;
            }
        }

        internal virtual async Task<int> EditTenant(Tenant tenantToEdit)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    UPDATE [Tenants]
                    SET 
                        [Alias] = @Alias,
                        [Name] = @Name
                    WHERE
                        [Guid] = @Guid;
                ";

                var affectedRows = await connection.ExecuteAsync(query,
                    new {@Alias = tenantToEdit.Alias, @Name = tenantToEdit.Name, @Guid = tenantToEdit.Guid.ToString()});

                return affectedRows;
            }
        }

        internal virtual async Task<int> DeleteTenant(Guid guid)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    DELETE FROM [Tenants]
                    WHERE [Guid] = @Guid;
                ";
                
                var affectedRows = await connection.ExecuteAsync(query, new {@Guid = guid.ToString()});

                return affectedRows;
            }
        }
    }
}