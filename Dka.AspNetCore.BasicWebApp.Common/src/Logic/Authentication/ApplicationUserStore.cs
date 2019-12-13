using System;
using System.Threading;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Authentication
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>
    {
        private readonly UserRepository _userRepository;
        
        public ApplicationUserStore(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public void Dispose()
        {
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.FromResult(user.UserName);
        }

        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.FromResult(user.UserName);
        }

        public async Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            
            await UpdateAsync(user, cancellationToken);
        }

        public async  Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.NormalizedUserName);
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;

            await UpdateAsync(user, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            return await _userRepository.CreateAsync(user);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            return await _userRepository.DeleteAsync(user);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userIdAsString, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.FindByIdAsync(int.Parse(userIdAsString));
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.FindByNameAsync(normalizedUserName);
        }
    }
}