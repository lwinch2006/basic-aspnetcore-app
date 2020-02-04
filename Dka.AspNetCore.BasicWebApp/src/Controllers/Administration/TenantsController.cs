using System;
using System.Collections;
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
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Services.ModelState;
using Dka.AspNetCore.BasicWebApp.ViewModels.Tenants;
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
            var tenantsViewModel = (IEnumerable<TenantViewModel>)new List<TenantViewModel>();
            
            try
            {
                var tenantsContract = await _internalApiClient.GetTenants();
                tenantsViewModel = _mapper.Map<IEnumerable<TenantViewModel>>(tenantsContract);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, _logger, HttpContext, ex);
            }

            return View("~/Views/Administration/Tenants/TenantList.cshtml", tenantsViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet("{guid}")]
        [ActionName("details")]
        public async Task<IActionResult> GetByGuid([FromRoute]Guid guid)
        {
            TenantViewModel tenantViewModel = null;

            try
            {
                var tenant = await _internalApiClient.GetTenantByGuid(guid);
                tenantViewModel = _mapper.Map<TenantViewModel>(tenant);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.ReadItemFailed, _logger, HttpContext, ex, guid);
            }

            return View("~/Views/Administration/Tenants/TenantDetails.cshtml", tenantViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [HttpGet]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant()
        {
            var newTenant = new ViewModels.Tenants.NewTenantViewModel();

            return await Task.FromResult(View("~/Views/Administration/Tenants/CreateNewTenant.cshtml", newTenant));
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant([Bind("Alias", "Name")] NewTenantViewModel newTenantViewModel)
        {
            if (!ModelState.IsValid || newTenantViewModel == null)
            {
                _logger.LogWarning(LoggingEvents.CreateItemBadData, "Empty tenant cannot be created. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View("~/Views/Administration/Tenants/CreateNewTenant.cshtml", newTenantViewModel);
            }

            try
            {
                var newTenantApiContract = _mapper.Map<NewTenantContract>(newTenantViewModel);

                var newTenantGuid = await _internalApiClient.CreateNewTenant(newTenantApiContract);

                return RedirectToAction("details", new { Guid = newTenantGuid });
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.CreateItemFailed, _logger, HttpContext, ex, newTenantViewModel.Name);
            }

            return View("~/Views/Administration/Tenants/CreateNewTenant.cshtml", newTenantViewModel);
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [HttpGet("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenantDetails([FromRoute]Guid guid)
        {
            EditTenantViewModel editTenantViewModel = null;

            try
            {
                var tenant = await _internalApiClient.GetTenantByGuid(guid);
                editTenantViewModel = _mapper.Map<EditTenantViewModel>(tenant);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.UpdateItemFailed, _logger, HttpContext, ex, guid);
            }

            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", editTenantViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [ValidateAntiForgeryToken]
        [HttpPost("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenantDetails([FromRoute] Guid guid, EditTenantViewModel editTenantViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemBadData, "Edit tenant model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", editTenantViewModel);
            }            
            
            try
            {
                var editTenantContract = _mapper.Map<EditTenantContract>(editTenantViewModel);
                
                await _internalApiClient.EditTenant(guid, editTenantContract);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.UpdateItemFailed, _logger, HttpContext, ex, guid);
            }

            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", editTenantViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Delete)]
        [ValidateAntiForgeryToken]
        [HttpPost("{guid}")]
        [ActionName("delete")]
        public async Task<IActionResult> DeleteTenant(Guid guid, EditTenantViewModel editTenantViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemBadData, "Delete tenant model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", editTenantViewModel);
            }
            
            try
            {
                await _internalApiClient.DeleteTenant(guid);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.DeleteItemFailed, _logger, HttpContext, ex, guid);
            }
            
            return View("~/Views/Administration/Tenants/EditTenantDetails.cshtml", editTenantViewModel);
        }
    }
}