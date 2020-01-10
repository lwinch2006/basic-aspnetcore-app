using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Microsoft.AspNetCore.Identity;

[assembly: InternalsVisibleTo("Dka.AspNetCore.BasicWebApp.Common.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseConnectionFactory _databaseConnectionFactory;
        
        public UserRepository(DatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        internal async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Id]
                          ,[UserName]
                          ,[NormalizedUserName]
                          ,[Email]
                          ,[NormalizedEmail]
                          ,[EmailConfirmed]
                          ,[PhoneNumber]
                          ,[PhoneNumberConfirmed]
                          ,[PasswordHash]
                          ,[SecurityStamp]
                          ,[ConcurrencyStamp]
                          ,[AccessFailedCount]
                          ,[LockoutEnabled]
                          ,[LockoutEnd]
                          ,[TwoFactorEnabled]
                          ,[Guid]
                    FROM [Users]
                    WHERE [Id] = @UserId
                ";

                var applicationUser = await connection.QuerySingleOrDefaultAsync<ApplicationUser>(query, new { @UserId = userId });

                return applicationUser;
            } 
        }

        internal async Task<ApplicationUser> FindByNameAsync(string normalizedUserName)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Id]
                          ,[UserName]
                          ,[NormalizedUserName]
                          ,[Email]
                          ,[NormalizedEmail]
                          ,[EmailConfirmed]
                          ,[PhoneNumber]
                          ,[PhoneNumberConfirmed]
                          ,[PasswordHash]
                          ,[SecurityStamp]
                          ,[ConcurrencyStamp]
                          ,[AccessFailedCount]
                          ,[LockoutEnabled]
                          ,[LockoutEnd]
                          ,[TwoFactorEnabled]
                          ,[Guid]
                    FROM [Users]
                    WHERE [NormalizedUserName] = @NormalizedUserName
                ";

                var applicationUser = await connection.QuerySingleOrDefaultAsync<ApplicationUser>(query, new { @NormalizedUserName = normalizedUserName });

                return applicationUser;
            }            
        }

        internal async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    DELETE
                    FROM [Users]
                    WHERE [Id] = @Id
                ";

                var result = await connection.ExecuteAsync(query, new {@Id = user.Id});

                return result == 1 ? 
                    IdentityResult.Success : IdentityResult.Failed();
            }
        }

        internal async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    UPDATE [Users]
                    SET [UserName] = @UserName
                        ,[NormalizedUserName] = @NormalizedUserName
                        ,[Email] = @Email
                        ,[NormalizedEmail] = @NormalizedEmail
                        ,[EmailConfirmed] = @EmailConfirmed
                        ,[PhoneNumber] = @PhoneNumber
                        ,[PhoneNumberConfirmed] = @PhoneNumberConfirmed
                        ,[PasswordHash] = @PasswordHash
                        ,[SecurityStamp] = @SecurityStamp
                        ,[ConcurrencyStamp] = @ConcurrencyStamp
                        ,[AccessFailedCount] = @AccessFailedCount
                        ,[LockoutEnabled] = @LockoutEnabled
                        ,[LockoutEnd] = @LockoutEnd
                        ,[TwoFactorEnabled] = @TwoFactorEnabled
                        ,[Guid] = @Guid
                    WHERE [Id] = @Id
                ";

                var result = await connection.ExecuteAsync(query, 
                    new
                    {
                        @Id = user.Id,
                        @UserName = user.UserName,
                        @NormalizedUserName = user.NormalizedUserName,
                        @Email = user.Email,
                        @NormalizedEmail = user.NormalizedEmail,
                        @EmailConfirmed = user.EmailConfirmed,
                        @PhoneNumber = user.PhoneNumber,
                        @PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        @PasswordHash = user.PasswordHash,
                        @SecurityStamp = user.SecurityStamp,
                        @ConcurrencyStamp = user.ConcurrencyStamp,
                        @AccessFailedCount = user.AccessFailedCount,
                        @LockoutEnabled = user.LockoutEnabled,
                        @LockoutEnd = user.LockoutEnd,
                        @TwoFactorEnabled = user.TwoFactorEnabled,
                        @Guid = user.Guid
                    });

                return result == 1 ? 
                    IdentityResult.Success : IdentityResult.Failed();
            }             
        }

        internal async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    INSERT INTO [Users] (
                        [UserName]
                        ,[NormalizedUserName]
                        ,[Email]
                        ,[NormalizedEmail]
                        ,[EmailConfirmed]
                        ,[PhoneNumber]
                        ,[PhoneNumberConfirmed]
                        ,[PasswordHash]
                        ,[SecurityStamp]
                        ,[ConcurrencyStamp]
                        ,[AccessFailedCount]
                        ,[LockoutEnabled]
                        ,[LockoutEnd]
                        ,[TwoFactorEnabled]
                        ,[Guid])
                    VALUES (
                        @UserName
                        ,@NormalizedUserName
                        ,@Email
                        ,@NormalizedEmail
                        ,@EmailConfirmed
                        ,@PhoneNumber
                        ,@PhoneNumberConfirmed
                        ,@PasswordHash
                        ,@SecurityStamp
                        ,@ConcurrencyStamp
                        ,@AccessFailedCount
                        ,@LockoutEnabled
                        ,@LockoutEnd
                        ,@TwoFactorEnabled
                        ,@Guid
                    );

                    SELECT SCOPE_IDENTITY();
                ";

                var result = await connection.QuerySingleOrDefaultAsync<int>(query, 
                    new
                    {
                        @UserName = user.UserName,
                        @NormalizedUserName = user.NormalizedUserName,
                        @Email = user.Email,
                        @NormalizedEmail = user.NormalizedEmail,
                        @EmailConfirmed = user.EmailConfirmed,
                        @PhoneNumber = user.PhoneNumber,
                        @PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        @PasswordHash = user.PasswordHash,
                        @SecurityStamp = user.SecurityStamp,
                        @ConcurrencyStamp = user.ConcurrencyStamp,
                        @AccessFailedCount = user.AccessFailedCount,
                        @LockoutEnabled = user.LockoutEnabled,
                        @LockoutEnd = user.LockoutEnd,
                        @TwoFactorEnabled = user.TwoFactorEnabled,
                        @Guid = user.Guid
                    });

                if (result == 0)
                {
                    return IdentityResult.Failed();
                }
                
                user.Id = result;
                return IdentityResult.Success;
            }   
        }

        internal async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    DECLARE @RoleId INT = 0;

                    SELECT @RoleId = [Id]
                    FROM [Roles]
                    WHERE [Name] = @RoleName

                    IF (@RoleId > 0)
                    BEGIN
                        INSERT INTO [UserRoles] ([UserId], [RoleId])
                        VALUES (@UserId, @RoleId);
                    END;    
                ";
                
                var result = await connection.ExecuteAsync(query, 
                    new
                    {
                        @UserId = user.Id,
                        @RoleName = roleName,
                    });
            }
        }

        internal async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    DECLARE @RoleId INT = 0;

                    SELECT @RoleId = [Id]
                    FROM [Roles]
                    WHERE [Name] = @RoleName

                    IF (@RoleId > 0)
                    BEGIN
                        DELETE
                        FROM [UserRoles]
                        WHERE [UserId] = @UserId AND [RoleId] = @RoleId
                    END;
                ";

                var result = await connection.ExecuteAsync(query,
                    new
                    {
                        @UserId = user.Id,
                        @RoleName = roleName,
                    });
            }
        }

        internal async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Name]
                    FROM [Roles]
                    WHERE [Id] IN (SELECT [RoleId] FROM [UserRoles] WHERE [UserId] = @UserId);
                ";

                var result = await connection.QueryAsync<string>(query,
                    new
                    {
                        @UserId = user.Id
                    });

                return result.ToList();
            }
        }

        internal async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [UserId]
                    FROM [UserRoles]
                    WHERE [UserId] = @UserId AND [RoleId] IN (SELECT [Id] FROM [Roles] WHERE [Name] = @RoleName);
                ";

                var result = await connection.QuerySingleOrDefaultAsync<int>(query,
                    new
                    {
                        @UserId = user.Id,
                        @RoleName = roleName,
                    });

                return result > 0;
            }
        }

        internal async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            using (var connection = _databaseConnectionFactory.GetConnection())
            {
                const string query = @"
                    SELECT [Id]
                          ,[UserName]
                          ,[NormalizedUserName]
                          ,[Email]
                          ,[NormalizedEmail]
                          ,[EmailConfirmed]
                          ,[PhoneNumber]
                          ,[PhoneNumberConfirmed]
                          ,[PasswordHash]
                          ,[SecurityStamp]
                          ,[ConcurrencyStamp]
                          ,[AccessFailedCount]
                          ,[LockoutEnabled]
                          ,[LockoutEnd]
                          ,[TwoFactorEnabled]
                          ,[Guid]
                    FROM [Users]
                    WHERE [Id] IN (SELECT [UserId] FROM [UserRoles] WHERE [RoleId] IN (SELECT [Id] FROM [Roles] WHERE [Name] = @RoleName));
                ";

                var result = await connection.QueryAsync<ApplicationUser>(query,
                    new
                    {
                        @RoleName = roleName,
                    });

                return result.ToList();
            }
        }
    }
}