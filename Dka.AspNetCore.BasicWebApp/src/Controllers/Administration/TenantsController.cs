using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Administration
{
    public class TenantsController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<TenantsController> _logger;

        private readonly HttpContext _httpContext;  
        
        public TenantsController(IInternalApiClient internalApiClient, IHttpContextAccessor httpContextAccessor, ILogger<TenantsController> logger)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }
        
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            var tenants = (IEnumerable<Tenant>)new List<Tenant>();
            
            try
            {
                tenants = await _internalApiClient.GetTenants();
            }
            catch (InternalApiClientException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

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