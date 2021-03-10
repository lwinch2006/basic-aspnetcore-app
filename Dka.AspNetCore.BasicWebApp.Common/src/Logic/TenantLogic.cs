using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic
{
    public interface ITenantLogic
    {
        Task<PagedResults<Tenant>> Get(Pagination pagination = null);
        Task<Tenant> Get(Guid guid);
        Task<Guid> Create(Tenant newTenantBo);
        Task<int> Update(Tenant tenantToEdit);
        Task<int> Delete(Guid guid);
    }

    public class TenantLogic : ITenantLogic
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantLogic(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<PagedResults<Tenant>> Get(Pagination pagination = null)
        {
            return await _tenantRepository.Get(pagination);
        }

        public async Task<Tenant> Get(Guid guid)
        {
            return await _tenantRepository.Get(guid);
        }

        public async Task<Guid> Create(Tenant newTenantBo)
        {
            return await _tenantRepository.Create(newTenantBo);
        }

        public async Task<int> Update(Tenant tenantToEdit)
        {
            return await _tenantRepository.Update(tenantToEdit);
        }

        public async Task<int> Delete(Guid guid)
        {
            return await _tenantRepository.Delete(guid);
        }
    }
}