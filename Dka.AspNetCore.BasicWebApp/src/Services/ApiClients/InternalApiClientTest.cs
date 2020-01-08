using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Tenant = Dka.AspNetCore.BasicWebApp.Common.Models.Tenants.Tenant;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public class InternalApiClientTest : IInternalApiClient
    {
        public InternalApiClientTest(HttpClient httpClient)
        {}
        
        public Task<string> GetPageNameAsync(string pageName)
        {
            return Task.FromResult(pageName);
        }

        public async Task<IEnumerable<Tenant>> GetTenants()
        {
            var dummyTenants = await Tenant.GetDummyTenantSet();

            return dummyTenants;
        }

        public async Task<Tenant> GetTenantByGuid(Guid guid)
        {
            var dummyTenant = (await Tenant.GetDummyTenantSet()).First();

            return dummyTenant;
        }

        public Task<bool> CheckApiOverallStatus()
        {
            return Task.FromResult(true);
        }

        public Task<bool> CheckApiReadyStatus()
        {
            return Task.FromResult(true);
        }

        public Task<bool> CheckApiLiveStatus()
        {
            return Task.FromResult(true);
        }

        public Task EditTenant(Guid guid, Common.Models.ApiContracts.Tenant tenantApiContract)
        {
            return Task.CompletedTask;
        }

        public async Task<Guid> CreateNewTenant(NewTenant newTenantApiContract)
        {
            return await Task.FromResult(new Guid("C78C30E4-620E-4E2C-8BAF-2A81BA8470A1"));
        }

        public Task DeleteTenant(Guid guid)
        {
            return Task.CompletedTask;
        }

        public Task<SignInResponseContract> SignIn(SignInRequestContract signInRequestContract)
        {
            throw new NotImplementedException();
        }

        public Task<SignOutResponseContract> SignOut(SignOutRequestContract signInRequestContract)
        {
            throw new NotImplementedException();
        }
    }
}