using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

        public async Task<bool> CheckApiOverallStatus()
        {
            return await CheckApiHealthByUrl("/health");
        }
        
        public async Task<bool> CheckApiReadyStatus()
        {
            return await CheckApiHealthByUrl("/health/ready");
        }
        
        public async Task<bool> CheckApiLiveStatus()
        {
            return await CheckApiHealthByUrl("/health/live");
        }

        private async Task<bool> CheckApiHealthByUrl(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var responseAsString = await response.Content.ReadAsStringAsync() ?? string.Empty;

                return responseAsString.Equals(HealthStatus.Healthy.ToString(), StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
    }
}