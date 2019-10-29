using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Administration
{
    public class TenantsController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        public TenantsController(IInternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
        }
        
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _internalApiClient.GetTenants();

            ViewData["Tenants"] = tenants;

            return View("~/Views/Administration/Tenants/TenantList.cshtml");
        }

        [HttpGet]
        [ActionName("index/{id}")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            return Ok();
        }
        
    }
}