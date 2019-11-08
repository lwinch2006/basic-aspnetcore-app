using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Tenants
{
    public class Tenant
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public static async Task<IEnumerable<Tenant>> GetDummyTenantSet()
        {
            var dummyTenants = new List<Tenant>
            {
                new Tenant { Guid = Guid.Parse("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), Alias = "umbrella", Name = "Umbrella Corporation"},
                new Tenant { Guid = Guid.Parse("F02E8F1F-0BBA-4049-9ED6-902F610DEE95"), Alias = "cyberdyne", Name = "Cyberdyne Systems"},
                new Tenant { Guid = Guid.Parse("5D71D117-F481-41B7-BE4A-AF0BB5A8A20E"), Alias = "ocp", Name = "OCP"}
            };

            return await Task.FromResult(dummyTenants);
        }
    }
}