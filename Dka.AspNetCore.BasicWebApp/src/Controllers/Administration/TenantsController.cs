using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Services.ModelState;
using Microsoft.AspNetCore.Authorization;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Administration
{
    [Route("Administration/[controller]/{action=Index}")]
    public class TenantsController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<TenantsController> _logger;

        private readonly IMapper _mapper;
        
        public TenantsController(
            IInternalApiClient internalApiClient, 
            ILogger<TenantsController> logger,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
            _mapper = mapper;
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
                ExceptionProcessor.Process(LoggingEvents.ReadItemsFailed, _logger, HttpContext, ex);
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
                ExceptionProcessor.Process(LoggingEvents.ReadItemFailed, _logger, HttpContext, ex, guid);
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
                _logger.LogWarning(LoggingEvents.CreateItemBadData, "Empty tenant cannot be created. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
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
                ExceptionProcessor.Process(LoggingEvents.CreateItemFailed, _logger, HttpContext, ex, newTenant.Name);
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
                ExceptionProcessor.Process(LoggingEvents.UpdateItemFailed, _logger, HttpContext, ex, guid);
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
                _logger.LogWarning(LoggingEvents.UpdateItemBadData, "Edit tenant model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToEditVm);
            }            
            
            try
            {
                var tenantToEditApiContract = _mapper.Map<Common.Models.ApiContracts.Tenant>(tenantToEditVm);
                
                await _internalApiClient.EditTenant(guid, tenantToEditApiContract);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.UpdateItemFailed, _logger, HttpContext, ex, guid);
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
                _logger.LogWarning(LoggingEvents.DeleteItemBadData, "Delete tenant model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToDeleteVm);
            }
            
            try
            {
                await _internalApiClient.DeleteTenant(tenantToDeleteVm.Guid);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.DeleteItemFailed, _logger, HttpContext, ex, guid);
            }
            
            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", tenantToDeleteVm);
        }
    }
}