using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Microsoft.AspNetCore.Identity;

[assembly: InternalsVisibleTo("Dka.AspNetCore.BasicWebApp.Common.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public class RoleRepository
    {
        private readonly DatabaseConnectionFactory _databaseConnectionFactory;
        
        public RoleRepository(DatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        internal async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    INSERT INTO [Roles] (
                        [Name]
                        ,[NormalizedName]
                        ,[ConcurrencyStamp]
                        ,[Guid])
                    VALUES (
                        @Name
                        ,@NormalizedName
                        ,@ConcurrencyStamp
                        ,@Guid
                    );

                    SELECT SCOPE_IDENTITY();
                ";

                var result = await connection.QuerySingleOrDefaultAsync<int>(query, 
                    new
                    {
                        @Name = role.Name,
                        @NormalizedName = role.NormalizedName,
                        @ConcurrencyStamp = role.ConcurrencyStamp,
                        @Guid = role.Guid
                    });

                if (result == 0)
                {
                    return IdentityResult.Failed();
                }
                
                role.Id = result;
                return IdentityResult.Success;
            }
        }

        internal async Task<IdentityResult> UpdateAsync(ApplicationRole role)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    UPDATE [Roles]
                    SET [Name] = @Name
                        ,[NormalizedName] = @NormalizedName
                        ,[ConcurrencyStamp] = @ConcurrencyStamp
                        ,[Guid] = @Guid
                    WHERE [Id] = @Id

                ";
                
                var result = await connection.ExecuteAsync(query, 
                    new
                    {
                        @Id = role.Id,
                        @Name = role.Name,
                        @NormalizedName = role.NormalizedName,
                        @ConcurrencyStamp = role.ConcurrencyStamp,
                        @Guid = role.Guid
                    });

                return result == 1 ? 
                    IdentityResult.Success : IdentityResult.Failed();
            }
        }

        internal async Task<IdentityResult> DeleteAsync(ApplicationRole role)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    DELETE
                    FROM [Roles]
                    WHERE [Id] = @Id

                ";
                
                var result = await connection.ExecuteAsync(query, 
                    new
                    {
                        @Id = role.Id
                    });

                return result == 1 ? 
                    IdentityResult.Success : IdentityResult.Failed();
            }
        }

        internal async Task<ApplicationRole> FindByIdAsync(int roleId)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Id]
                          ,[Name]
                          ,[NormalizedName]
                          ,[ConcurrencyStamp]
                          ,[Guid]
                    FROM [Roles]
                    WHERE [Id] = @Id

                ";
                
                var result = await connection.QuerySingleOrDefaultAsync<ApplicationRole>(query, 
                    new
                    {
                        @Id = roleId
                    });

                return result;            
            }
        }

        internal async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Id]
                          ,[Name]
                          ,[NormalizedName]
                          ,[ConcurrencyStamp]
                          ,[Guid]
                    FROM [Roles]
                    WHERE [NormalizedName] = @NormalizedName

                ";
                
                var result = await connection.QuerySingleOrDefaultAsync<ApplicationRole>(query, 
                    new
                    {
                        @NormalizedName = normalizedRoleName
                    });

                return result;            
            }
        }
    }
}