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
                ";

                var result = await connection.ExecuteAsync(query, 
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

                return result == 1 ? 
                    IdentityResult.Success : IdentityResult.Failed();
            }   
        }
    }
}