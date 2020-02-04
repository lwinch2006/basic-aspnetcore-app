using System;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants
{
    public class TenantContract
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }        
    }
}