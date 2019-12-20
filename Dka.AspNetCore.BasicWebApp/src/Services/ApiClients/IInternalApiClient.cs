using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public interface IInternalApiClient
    {
        Task<string> GetPageNameAsync(string pageName);

        Task<IEnumerable<Tenant>> GetTenants();
        
        Task<Tenant> GetTenantByGuid(Guid guid);

        Task<bool> CheckApiOverallStatus();

        Task<bool> CheckApiReadyStatus();

        Task<bool> CheckApiLiveStatus();

        Task EditTenant(Guid guid, Common.Models.ApiContracts.Tenant tenantApiContract);

        Task<Guid> CreateNewTenant(Common.Models.ApiContracts.NewTenant newTenantApiContract);

        Task DeleteTenant(Guid guid);

        Task<SignInResponseContract> Login(SignInRequestContract signInRequestContract);
        
        Task<SignOutResponseContract> Logout(SignOutRequestContract signInRequestContract);
    }
}