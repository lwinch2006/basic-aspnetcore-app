using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Users;
using Tenant = Dka.AspNetCore.BasicWebApp.Common.Models.Tenants.Tenant;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public class InternalApiClientTest : IInternalApiClient
    {
        private readonly IMapper _mapper;

        public InternalApiClientTest(HttpClient httpClient, IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public Task<string> GetPageNameAsync(string pageName)
        {
            return Task.FromResult(pageName);
        }

        public async Task<IEnumerable<TenantContract>> GetTenants()
        {
            var dummyTenants = await Tenant.GetDummyTenantSet();
            var dummyTenantsContract = _mapper.Map<IEnumerable<TenantContract>>(dummyTenants); 
            
            return dummyTenantsContract;
        }

        public Task<IEnumerable<ApplicationUserContract>> GetApplicationUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<TenantContract> GetTenantByGuid(Guid guid)
        {
            var dummyTenant = (await Tenant.GetDummyTenantSet()).First();
            var dummyTenantContract = _mapper.Map<TenantContract>(dummyTenant); 
            
            return dummyTenantContract;
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

        public Task EditTenant(Guid guid, EditTenantContract editTenantContract)
        {
            return Task.CompletedTask;
        }

        public async Task<Guid> CreateNewTenant(NewTenantContract newTenantContract)
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