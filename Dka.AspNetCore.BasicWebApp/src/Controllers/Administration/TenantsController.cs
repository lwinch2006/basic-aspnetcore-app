using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Administration
{
    [Route("Administration/[controller]/{action=Index}")]
    public class TenantsController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<TenantsController> _logger;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        private readonly IAuthorizationService _authorizationService;
        
        public TenantsController(
            IInternalApiClient internalApiClient, 
            IHttpContextAccessor httpContextAccessor, 
            ILogger<TenantsController> logger,
            IMapper mapper,
            IAuthorizationService authorizationService)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            var tenants = (IEnumerable<Tenant>)new List<Tenant>();
            
            try
            {
                tenants = await _internalApiClient.GetTenants();
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View("~/Views/Administration/Tenants/TenantList.cshtml", tenants);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet("{guid}")]
        [ActionName("details")]
        public async Task<IActionResult> GetByGuid([FromRoute]Guid guid)
        {
            ViewModels.Tenants.Tenant tenantVm = null;

            try
            {
                var tenantBo = await _internalApiClient.GetTenantByGuid(guid);

                tenantVm = _mapper.Map<ViewModels.Tenants.Tenant>(tenantBo);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View("~/Views/Administration/Tenants/TenantDetails.cshtml", tenantVm);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [HttpGet]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant()
        {
            var newTenant = new ViewModels.Tenants.NewTenant();

            return await Task.FromResult(View("~/Views/Administration/Tenants/CreateNewTenant.cshtml", newTenant));
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant([Bind("Alias", "Name")] ViewModels.Tenants.NewTenant newTenant)
        {
            if (!ModelState.IsValid || newTenant == null)
            {
                return View("~/Views/Administration/Tenants/CreateNewTenant.cshtml", newTenant);
            }

            try
            {
                var newTenantApiContract = _mapper.Map<Common.Models.ApiContracts.NewTenant>(newTenant);

                var newTenantGuid = await _internalApiClient.CreateNewTenant(newTenantApiContract);

                return RedirectToAction("details", new { Guid = newTenantGuid });
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View("~/Views/Administration/Tenants/CreateNewTenant.cshtml", newTenant);
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [HttpGet("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenantDetails([FromRoute]Guid guid)
        {
            ViewModels.Tenants.Tenant tenantVm = null;

            try
            {
                var tenantBo = await _internalApiClient.GetTenantByGuid(guid);
                
                tenantVm = _mapper.Map<ViewModels.Tenants.Tenant>(tenantBo);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantVm);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [ValidateAntiForgeryToken]
        [HttpPost("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenantDetails([FromRoute] Guid guid, [Bind("Alias", "Name", "Guid")] ViewModels.Tenants.Tenant tenantToEditVm)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToEditVm);
            }            
            
            try
            {
                if (guid != tenantToEditVm?.Guid)
                {
                    throw new TenantNotFoundException();
                }

                var tenantToEditApiContract = _mapper.Map<Common.Models.ApiContracts.Tenant>(tenantToEditVm);
                
                await _internalApiClient.EditTenant(guid, tenantToEditApiContract);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToEditVm);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Delete)]
        [ValidateAntiForgeryToken]
        [HttpPost("{guid}")]
        [ActionName("delete")]
        public async Task<IActionResult> DeleteTenant([FromRoute]Guid guid, [Bind("Alias", "Name", "Guid")] ViewModels.Tenants.Tenant tenantToDeleteVm)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToDeleteVm);
            }
            
            try
            {
                if (guid != tenantToDeleteVm?.Guid)
                {
                    throw new TenantNotFoundException();
                }

                await _internalApiClient.DeleteTenant(tenantToDeleteVm.Guid);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }
            
            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToDeleteVm);
        }
    }
}