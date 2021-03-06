using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Users;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

        public async Task<PagedResults<TenantContract>> GetTenants(Common.Models.Pagination.Pagination pagination = null)
        {
            try
            {
                var relativeUrl = "/Administration/Tenants/";
                
                if (pagination != null)
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    query[nameof(pagination.PageIndex)] = pagination.PageIndex.ToString();
                    query[nameof(pagination.PageSize)] = pagination.PageSize.ToString();
                    relativeUrl = $"{relativeUrl}?{query}";
                }

                var response = await _httpClient.GetAsync(relativeUrl);
                response.EnsureSuccessStatusCode();

                var tenants = await response.Content.ReadAsAsync<PagedResults<TenantContract>>();

                return tenants;
            }
            catch (Exception ex)
            {
                throw new ApiConnectionException(ex);
            }
        }

        public async Task<IEnumerable<ApplicationUserContract>> GetApplicationUsers()
        {
            return await Task.FromResult(new List<ApplicationUserContract>());
        }
        
        public async Task<TenantContract> GetTenantByGuid(Guid guid)
        {
            HttpResponseMessage response = null;
            
            try
            {
                response = await _httpClient.GetAsync($"/Administration/Tenants/{guid}");

                response.EnsureSuccessStatusCode();

                var tenant = await response.Content.ReadAsAsync<TenantContract>();

                return tenant;
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
        
        public async Task<Guid> CreateNewTenant(NewTenantContract newTenantContract)
        {
            HttpResponseMessage response = null;
            
            try
            {
                var newTenantApiContractAsJson = JsonSerializer.Serialize(newTenantContract);
                
                var newTenantApiContractAsContent = new StringContent(newTenantApiContractAsJson, Encoding.UTF8, "application/json");
                
                response = await _httpClient.PostAsync("/Administration/Tenants/", newTenantApiContractAsContent);

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

        public async Task EditTenant(Guid guid, EditTenantContract editTenantContract)
        {
            HttpResponseMessage response = null;
            
            try
            {
                var editTenantContractAsJson = JsonSerializer.Serialize(editTenantContract);
                
                var editTenantContractAsContent = new StringContent(editTenantContractAsJson, Encoding.UTF8, "application/json");

                response = await _httpClient.PutAsync($"/Administration/Tenants/{guid}", editTenantContractAsContent);

                response.EnsureSuccessStatusCode();
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

        public async Task DeleteTenant(Guid guid)
        {
            HttpResponseMessage response = null;
            
            try
            {
                response = await _httpClient.DeleteAsync($"/Administration/Tenants/{guid}");

                response.EnsureSuccessStatusCode();
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