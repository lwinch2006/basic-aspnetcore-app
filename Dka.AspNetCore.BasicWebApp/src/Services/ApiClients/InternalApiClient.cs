using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Tenant = Dka.AspNetCore.BasicWebApp.Common.Models.Tenants.Tenant;

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

        public async Task<Tenant> GetTenantByGuid(Guid guid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/Administration/Tenants/Details/{guid}");

                response.EnsureSuccessStatusCode();

                var tenant = await response.Content.ReadAsAsync<Tenant>();

                return tenant;
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

        public Task UpdateTenant(Common.Models.ApiContracts.Tenant tenantApiContract)
        {
            return Task.CompletedTask;
        }

        public async Task<Guid> CreateNewTenant(NewTenant newTenantApiContract)
        {
            try
            {
                var newTenantApiContractAsJson = JsonSerializer.Serialize(newTenantApiContract);
                
                var newTenantApiContractAsContent = new StringContent(newTenantApiContractAsJson, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/Administration/Tenants/new", newTenantApiContractAsContent);

                response.EnsureSuccessStatusCode();

                var tenantGuid = await response.Content.ReadAsAsync<Guid>();

                return tenantGuid;
            }
            catch (Exception ex)
            {
                throw new InternalApiClientException("Internal API client exception", ex);
            }
        }

        public Task DeleteTenant(Guid guid)
        {
            return Task.CompletedTask;
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