using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Tenant = Dka.AspNetCore.BasicWebApp.Common.Models.Tenants.Tenant;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public class InternalApiClient : IInternalApiClient
    {
        private readonly HttpClient _httpClient;

        public InternalApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;

            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var accessToken =
                    httpContext.User.Claims.Single(record => record.Type.Equals(ClaimsCustomTypes.AccessToken)).Value;
                
                _httpClient.DefaultRequestHeaders.Add(HttpHeaders.Authorization, new[] { $"{AuthorizationConstants.Bearer} {accessToken}" });
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
                throw new ApiConnectionException(ex);
            }
        }

        public async Task<Tenant> GetTenantByGuid(Guid guid)
        {
            HttpResponseMessage response = null;
            
            try
            {
                response = await _httpClient.GetAsync($"/Administration/Tenants/Details/{guid}");

                response.EnsureSuccessStatusCode();

                var tenant = await response.Content.ReadAsAsync<Tenant>();

                return tenant;
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                {
                    throw new ApiConnectionException(ex);
                }
                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new TenantNotFoundException(ex);
                    
                    default:
                        throw new ApiStatusCodeException(ex);
                }
            }
            catch (Exception ex)
            {
                throw new ApiConnectionException(ex);
            }
        }
        
        public async Task<Guid> CreateNewTenant(NewTenant newTenantApiContract)
        {
            HttpResponseMessage response = null;
            
            try
            {
                var newTenantApiContractAsJson = JsonSerializer.Serialize(newTenantApiContract);
                
                var newTenantApiContractAsContent = new StringContent(newTenantApiContractAsJson, Encoding.UTF8, "application/json");
                
                response = await _httpClient.PostAsync("/Administration/Tenants/new", newTenantApiContractAsContent);

                response.EnsureSuccessStatusCode();

                var tenantGuid = await response.Content.ReadAsAsync<Guid>();

                return tenantGuid;
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                {
                    throw new ApiConnectionException(ex);
                }
                
                throw new ApiStatusCodeException(ex);
            }            
            catch (Exception ex)
            {
                throw new ApiConnectionException(ex);
            }
        }

        public async Task EditTenant(Guid guid, Common.Models.ApiContracts.Tenant tenantToEditApiContract)
        {
            HttpResponseMessage response = null;
            
            try
            {
                var tenantToEditApiContractAsJson = JsonSerializer.Serialize(tenantToEditApiContract);
                
                var tenantToEditApiContractAsContent = new StringContent(tenantToEditApiContractAsJson, Encoding.UTF8, "application/json");

                response = await _httpClient.PutAsync($"/Administration/Tenants/edit/{guid}", tenantToEditApiContractAsContent);

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                {
                    throw new ApiConnectionException(ex);
                }
                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new TenantNotFoundException(ex);
                    
                    default:
                        throw new ApiStatusCodeException(ex);
                }
            }             
            catch (Exception ex)
            {
                throw new ApiConnectionException(ex);
            }
        }

        public async Task DeleteTenant(Guid guid)
        {
            HttpResponseMessage response = null;
            
            try
            {
                response = await _httpClient.DeleteAsync($"/Administration/Tenants/delete/{guid}");

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                {
                    throw new ApiConnectionException(ex);
                }
                
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new TenantNotFoundException(ex);
                    
                    default:
                        throw new ApiStatusCodeException(ex);
                }
            }             
            catch (Exception ex)
            {
                throw new ApiConnectionException(ex);
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
            catch (Exception)
            {
                return false;
            }            
        }

        public async Task<SignInResponseContract> SignIn(SignInRequestContract signInRequestContract)
        {
            HttpResponseMessage response = null;
            
            try
            {
                var signInRequestContractSerialized = JsonSerializer.Serialize(signInRequestContract);
                var signInRequestContractAsRequestContent = new StringContent(signInRequestContractSerialized, Encoding.UTF8, "application/json");
                
                response = await _httpClient.PostAsync(AuthenticationDefaults.LoginUrl, signInRequestContractAsRequestContent);
                response.EnsureSuccessStatusCode();
                
                var signInResponseContract = await response.Content.ReadAsAsync<SignInResponseContract>();

                return signInResponseContract;
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                {
                    throw new ApiConnectionException(ex);
                }
                    
                throw new ApiStatusCodeException(ex);
            }            
            catch (Exception ex)
            {
                throw new ApiConnectionException(ex);
            }
        }

        public async Task<SignOutResponseContract> SignOut(SignOutRequestContract signOutRequestContract)
        {
            return await Task.FromResult(new SignOutResponseContract());
        }
    }
}