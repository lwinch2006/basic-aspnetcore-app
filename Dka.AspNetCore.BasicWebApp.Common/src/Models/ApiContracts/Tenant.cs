using System;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts
{
    public class Tenant
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }        
    }
}