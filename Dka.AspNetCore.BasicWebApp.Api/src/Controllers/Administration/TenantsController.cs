using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration
{
    [Route("Administration/[controller]/{action=Index}")]
    public class TenantsController : Controller
    {
        private readonly TenantLogic _tenantLogic;

        private readonly IMapper _mapper;

        private readonly ILogger<TenantsController> _logger;

        public TenantsController(TenantLogic tenantLogic, IMapper mapper, ILogger<TenantsController> logger)
        {
            _tenantLogic = tenantLogic;
            _mapper = mapper;
            _logger = logger;
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ReadItems, "Getting all tenants.");
            
            var tenants = await _tenantLogic.GetAll();
            var tenantsContract = _mapper.Map<IEnumerable<TenantContract>>(tenants);
            
            return Ok(tenantsContract);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet("{guid}")]
        [ActionName("details")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            _logger.LogInformation(LoggingEvents.ReadItem, "Getting tenant with GUID {guid}.", guid);
            
            var tenant = await _tenantLogic.GetByGuid(guid);

            if (tenant == null)
            {
                _logger.LogWarning(LoggingEvents.ReadItemNotFound, "Tenant with GUID {guid} not found.", guid);
                return NotFound();
            }
            
            var tenantContract = _mapper.Map<TenantContract>(tenant);
            
            return Ok(tenantContract);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [HttpPost]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant([FromBody] NewTenantContract newTenantContract)
        {
            if (newTenantContract == null)
            {
                _logger.LogWarning(LoggingEvents.CreateItemBadData, "Empty tenant cannot be created.");
                return BadRequest();
            }
            
            try
            {
                _logger.LogInformation(LoggingEvents.CreateItem, "Creating new tenant with name {Name}.", newTenantContract.Name);

                var newTenant = _mapper.Map<Tenant>(newTenantContract);
                newTenant.Guid = await _tenantLogic.CreateNewTenant(newTenant);
                return Ok(newTenant.Guid);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.CreateItemFailed, _logger, ex, newTenantContract.Name);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [HttpPut("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenant(Guid guid, [FromBody] TenantContract tenantToEditContract)
        {
            if (guid != tenantToEditContract?.Guid)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemBadData, "Different tenant update attempt.");
                return BadRequest(tenantToEditContract);
            }             
            
            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Updating tenant with GUID {guid}.", guid);
                
                var tenantToEdit = _mapper.Map<Tenant>(tenantToEditContract);
                await _tenantLogic.EditTenant(tenantToEdit);
                return NoContent();
            }
            catch (BasicWebAppException ex)
            {
                var tenant = await _tenantLogic.GetByGuid(guid);

                if (tenant == null)
                {
                    _logger.LogWarning(LoggingEvents.UpdateItemNotFound, ex, "Tenant with GUID {guid} for updating not found.", guid);
                    return NotFound();
                }
                
                ExceptionProcessor.Process(LoggingEvents.UpdateItemFailed, _logger, ex, guid);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Delete)]
        [HttpDelete("{guid}")]
        [ActionName("delete")]
        public async Task<IActionResult> DeleteTenant(Guid guid)
        {
            _logger.LogInformation(LoggingEvents.ReadItem, "Deleting tenant with GUID {guid}.", guid);
            
            try
            {
                var tenantToDelete = await _tenantLogic.GetByGuid(guid);

                if (tenantToDelete == null)
                {
                    _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Tenant with GUID {guid} for deleting not found.", guid);
                    return NotFound();
                }
                
                await _tenantLogic.DeleteTenant(tenantToDelete.Guid);
                return Ok(tenantToDelete);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.UpdateItemFailed, _logger, ex, guid);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}