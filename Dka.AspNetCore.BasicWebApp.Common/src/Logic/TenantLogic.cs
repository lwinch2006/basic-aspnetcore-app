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

        public async Task<IEnumerable<Tenant>> GetAll()
        {
            var tenants = await _tenantRepository.GetAll();

            return tenants;
        }

        public async Task<Tenant> GetByGuid(Guid guid)
        {
            var tenant = await _tenantRepository.GetByGuid(guid);

            return tenant;
        }

        public async Task<Guid> CreateNewTenant(Tenant newTenantBo)
        {
            var newTenantGuid = await _tenantRepository.CreateNewTenant(newTenantBo);

            return newTenantGuid;
        }
    }
}