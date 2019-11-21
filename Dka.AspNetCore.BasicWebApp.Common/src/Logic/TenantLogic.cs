using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Repositories;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic
{
    public class TenantLogic
    {
        private readonly TenantRepository _tenantRepository;

        public TenantLogic(TenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public virtual async Task<IEnumerable<Tenant>> GetAll()
        {
            var tenants = await _tenantRepository.GetAll();

            return tenants;
        }

        public virtual async Task<Tenant> GetByGuid(Guid guid)
        {
            var tenant = await _tenantRepository.GetByGuid(guid);

            return tenant;
        }

        public virtual async Task<Guid> CreateNewTenant(Tenant newTenantBo)
        {
            var newTenantGuid = await _tenantRepository.CreateNewTenant(newTenantBo);

            return newTenantGuid;
        }

        public virtual async Task<int> EditTenant(Tenant tenantToEdit)
        {
            return await _tenantRepository.EditTenant(tenantToEdit);
        }

        public virtual async Task<int> DeleteTenant(Guid guid)
        {
            return await _tenantRepository.DeleteTenant(guid);
        }
    }
}