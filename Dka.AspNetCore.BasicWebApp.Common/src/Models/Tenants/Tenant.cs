using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Tenants
{
    public class Tenant
    {
        public Guid Guid { get; private set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public static async Task<IEnumerable<Tenant>> GetDummyTenantSet()
        {
            var dummyTenants = new List<Tenant>
            {
                new Tenant { Guid = Guid.NewGuid(), Alias = "umbrella", Name = "Umbrella Corporation"},
                new Tenant { Guid = Guid.NewGuid(), Alias = "cyberdyne", Name = "Cyberdyne Systems"},
                new Tenant { Guid = Guid.NewGuid(), Alias = "ocp", Name = "OCP"}
            };

            return await Task.FromResult(dummyTenants);
        }
    }
}