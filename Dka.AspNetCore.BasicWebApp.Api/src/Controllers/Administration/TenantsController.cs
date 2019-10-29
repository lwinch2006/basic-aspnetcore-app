using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration
{
    public class TenantsController : Controller
    {
        private readonly TenantLogic _tenantLogic;
        
        public TenantsController(TenantLogic tenantLogic)
        {
            _tenantLogic = tenantLogic;
        }
        
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _tenantLogic.GetAll();

            return Ok(tenants);
        }

        [HttpGet]
        [ActionName("index/{id}")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            var tenant = new Tenant();
            
            return Ok(tenant);
        }
    }
}