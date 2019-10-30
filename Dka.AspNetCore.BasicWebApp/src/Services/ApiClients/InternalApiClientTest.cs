using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;

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
    }
}