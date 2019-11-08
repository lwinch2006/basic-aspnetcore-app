using System;

namespace Dka.AspNetCore.BasicWebApp.ViewModels.Tenants
{
    public class Tenant
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }           
    }
}