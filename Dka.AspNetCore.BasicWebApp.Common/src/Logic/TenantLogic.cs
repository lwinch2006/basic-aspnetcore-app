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
    }
}