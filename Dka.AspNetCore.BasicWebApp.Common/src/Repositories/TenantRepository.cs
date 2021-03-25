using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Common.Repositories.Extensions;

[assembly: InternalsVisibleTo("Dka.AspNetCore.BasicWebApp.Common.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public interface ITenantRepository
    {
        Task<PagedResults<Tenant>> Get(Pagination pagination = null);
        Task<Tenant> Get(Guid guid);
        Task<Guid> Create(Tenant newTenantBo);
        Task<int> Update(Tenant tenantToEdit);
        Task<int> Delete(Guid guid);
        Task<bool> Exists(Guid guid);
    }

    public class TenantRepository : ITenantRepository
    {
        private readonly DatabaseConnectionFactory _databaseConnectionFactory;

        public TenantRepository(DatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<PagedResults<Tenant>> Get(Pagination pagination = null)
        {
            var dynamicParameters = new DynamicParameters();
            
            AddPagination(pagination, dynamicParameters);
            
            var query = $@"
                SELECT *
                FROM [Tenants]
                {Sql.OrderWithPossiblePagination(pagination, "[Name]")}
                
                SELECT COUNT([Id])
                FROM [Tenants]
            ";
            
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                using (var multipleQuery = await connection.QueryMultipleAsync(query, dynamicParameters))
                {
                    var tenants = await multipleQuery.ReadAsync<Tenant>();
                    var totalCount = multipleQuery.ReadFirst<int>();
                    
                    var result = new PagedResults<Tenant>
                    {
                        Items = tenants,
                        TotalCount = totalCount
                    };
                    
                    return result;
                }
            }
        }

        public async Task<Tenant> Get(Guid guid)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT *
                    FROM [Tenants]
                    WHERE Guid = @Guid
                ";

                var tenant = await connection.QuerySingleOrDefaultAsync<Tenant>(query, new { Guid = guid.ToString() });

                return tenant;
            }
        }

        public async Task<Guid> Create(Tenant newTenantBo)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                newTenantBo.Guid = Guid.NewGuid();
                
                const string query = @"
                    INSERT INTO [Tenants] ([Alias], [Name], [Guid], [CreatedOnUtc], [CreatedBy])
                    VALUES (@Alias, @Name, @Guid, @CreatedOnUtc, @CreatedBy);
                ";

                var affectedRows = await connection.ExecuteAsync(query, newTenantBo);

                return affectedRows == 1 ? newTenantBo.Guid : Guid.Empty;
            }
        }

        public async Task<int> Update(Tenant tenantToEdit)
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

                var affectedRows = await connection.ExecuteAsync(query, new {@Alias = tenantToEdit.Alias, @Name = tenantToEdit.Name, @Guid = tenantToEdit.Guid.ToString()});

                return affectedRows;
            }
        }

        public async Task<int> Delete(Guid guid)
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

        public async Task<bool> Exists(Guid guid)
        {
            const string query = @"
                SELECT COUNT(1)
                FROM [Tenants]
                WHERE [Guid] = @TenantGuid;
            ";

            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                var result = await connection.ExecuteScalarAsync<bool>(query, new {@TenantGuid = guid});
                return result;
            }
        }
        
        private void AddPagination(Pagination pagination, DynamicParameters dynamicParameters)
        {
            if (!(pagination?.PageSize > 0) || pagination.PageIndex < 0)
            {
                return;
            }
            
            var pageOffset = pagination.PageIndex * pagination.PageSize;
            dynamicParameters.Add("@PageOffset", pageOffset);
            dynamicParameters.Add("@PageSize", pagination.PageSize);
        }        
    }
}