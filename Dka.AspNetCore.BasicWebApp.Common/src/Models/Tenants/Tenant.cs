using System;
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
        public DateTime CreatedOnUtc { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public int? UpdatedBy { get; set; }

        public static async Task<IEnumerable<Tenant>> GetDummyTenantSet()
        {
            var dummyTenants = new List<Tenant>
            {
                new Tenant { Id = 1, Name = "Umbrella Corporation", Alias = "umbrella", Guid = new Guid("9D5CC1D7-EA23-43AB-8725-01D8EBF0B11C"), CreatedOnUtc = DateTime.UtcNow, CreatedBy = -1 },
                new Tenant { Id = 2, Name = "Cyberdyne Systems", Alias = "cyberdyne", Guid = new Guid("F02E8F1F-0BBA-4049-9ED6-902F610DEE95"), CreatedOnUtc = DateTime.UtcNow, CreatedBy = -1 },
                new Tenant { Id = 3, Name = "OCP", Alias = "ocp", Guid = new Guid("5D71D117-F481-41B7-BE4A-AF0BB5A8A20E"), CreatedOnUtc = DateTime.UtcNow, CreatedBy = -1 }
            };

            return await Task.FromResult(dummyTenants);
        }
    }
}