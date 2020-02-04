using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Users;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public interface IInternalApiClient
    {
        Task<IEnumerable<TenantContract>> GetTenants();
        
        Task<IEnumerable<ApplicationUserContract>> GetApplicationUsers();

        Task<TenantContract> GetTenantByGuid(Guid guid);

        Task<bool> CheckApiOverallStatus();

        Task<bool> CheckApiReadyStatus();

        Task<bool> CheckApiLiveStatus();

        Task EditTenant(Guid guid, EditTenantContract editTenantContract);

        Task<Guid> CreateNewTenant(NewTenantContract newTenantContract);

        Task DeleteTenant(Guid guid);

        Task<SignInResponseContract> SignIn(SignInRequestContract signInRequestContract);
        
        Task<SignOutResponseContract> SignOut(SignOutRequestContract signInRequestContract);
    }
}