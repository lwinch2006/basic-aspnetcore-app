using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;

namespace Dka.AspNetCore.BasicWebApp.Common.Repositories
{
    public class TenantRepository
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public TenantRepository(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        internal async Task<IEnumerable<Tenant>> GetAll()
        {
            var dummyTenants = await Tenant.GetDummyTenantSet();

            return dummyTenants;
        }
    }
}