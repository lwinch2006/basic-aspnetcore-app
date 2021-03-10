using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Areas.Administration.ViewModels.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Services.HttpContext;
using Dka.AspNetCore.BasicWebApp.Services.ModelState;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;

namespace Dka.AspNetCore.BasicWebApp.Areas.Administration.Controllers
{
    [Area(CommonConstants.Controllers.Areas.Administration)]
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
        public async Task<IActionResult> Index(PaginationRequestViewModel paginationRequestViewModel)
        {
            var pagedTenantsViewModel = PagedResultsViewModel<TenantViewModel>.InitEmpty();
            
            try
            {
                paginationRequestViewModel.PageSize ??= HttpContext.TryGetPageSizeFromCookie();
                var pagination = _mapper.Map<Pagination>(paginationRequestViewModel);
                var pagedTenants = await _internalApiClient.GetTenants(pagination);
                pagedTenantsViewModel = _mapper.Map<PagedResultsViewModel<TenantViewModel>>(pagedTenants);
                _mapper.Map(pagination, pagedTenantsViewModel);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, _logger, HttpContext, ex);
            }

            return View(pagedTenantsViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        public async Task<IActionResult> Details(Guid id)
        {
            TenantViewModel tenantViewModel = null;

            try
            {
                var tenant = await _internalApiClient.GetTenantByGuid(id);
                tenantViewModel = _mapper.Map<TenantViewModel>(tenant);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.ReadItemFailed, _logger, HttpContext, ex, id);
            }

            return View(tenantViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        public IActionResult Create()
        {
            return View();
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(NewTenantViewModel newTenantViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.CreateItemBadData, "Empty tenant cannot be created. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View(newTenantViewModel);
            }

            try
            {
                var newTenantApiContract = _mapper.Map<NewTenantContract>(newTenantViewModel);

                var newTenantGuid = await _internalApiClient.CreateNewTenant(newTenantApiContract);

                return RedirectToAction(nameof(Details), new { Id = newTenantGuid });
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.CreateItemFailed, _logger, HttpContext, ex, newTenantViewModel.Name);
            }

            return View(newTenantViewModel);
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        public async Task<IActionResult> Update(Guid id)
        {
            EditTenantViewModel editTenantViewModel = null;

            try
            {
                var tenant = await _internalApiClient.GetTenantByGuid(id);
                editTenantViewModel = _mapper.Map<EditTenantViewModel>(tenant);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.UpdateItemFailed, _logger, HttpContext, ex, id);
            }

            return View(editTenantViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, EditTenantViewModel editTenantViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemBadData, "Edit tenant model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View(editTenantViewModel);
            }            
            
            try
            {
                var editTenantContract = _mapper.Map<EditTenantContract>(editTenantViewModel);
                
                await _internalApiClient.EditTenant(id, editTenantContract);

                return RedirectToAction(nameof(Index));
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.UpdateItemFailed, _logger, HttpContext, ex, id);
            }

            return View(editTenantViewModel);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, EditTenantViewModel editTenantViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.DeleteItemBadData, "Delete tenant model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View(nameof(Update), editTenantViewModel);
            }
            
            try
            {
                await _internalApiClient.DeleteTenant(id);

                return RedirectToAction("index");
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.DeleteItemFailed, _logger, HttpContext, ex, id);
            }
            
            return View(nameof(Update), editTenantViewModel);
        }
    }
}