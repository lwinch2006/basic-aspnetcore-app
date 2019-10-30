using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public class InternalApiClient : IInternalApiClient
    {
        private readonly HttpClient _httpClient;

        public InternalApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPageNameAsync(string pageName)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"/Pages/GetPageName?pagename={pageName}");

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new InternalApiClientException("Internal API client exception", ex);
            }
        }

        public async Task<IEnumerable<Tenant>> GetTenants()
        {
            try
            {
                var response = await _httpClient.GetAsync("/Administration/Tenants");

                response.EnsureSuccessStatusCode();

                var tenants = await response.Content.ReadAsAsync<IEnumerable<Tenant>>();

                return tenants;
            }
            catch (Exception ex)
            {
                throw new InternalApiClientException("Internal API client exception", ex);
            }
        }
    }
}