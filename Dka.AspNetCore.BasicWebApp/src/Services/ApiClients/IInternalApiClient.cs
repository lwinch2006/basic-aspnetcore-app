using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;

namespace Dka.AspNetCore.BasicWebApp.Services.ApiClients
{
    public interface IInternalApiClient
    {
        Task<string> GetPageNameAsync(string pageName);

        Task<IEnumerable<Tenant>> GetTenants();

        Task<bool> CheckApiOverallStatus();

        Task<bool> CheckApiReadyStatus();

        Task<bool> CheckApiLiveStatus();
    }
}