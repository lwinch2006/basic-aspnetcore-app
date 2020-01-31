using System;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
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
            return Ok(tenants);
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
            
            return Ok(tenant);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [HttpPost]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant([FromBody] Common.Models.ApiContracts.NewTenant newTenantApiContract)
        {
            if (newTenantApiContract == null)
            {
                _logger.LogWarning(LoggingEvents.CreateItemBadData, "Empty tenant cannot be created.");
                return BadRequest();
            }
            
            try
            {
                _logger.LogInformation(LoggingEvents.CreateItem, "Creating new tenant with name {Name}.", newTenantApiContract.Name);

                var newTenantBo = _mapper.Map<Tenant>(newTenantApiContract);
                newTenantBo.Guid = await _tenantLogic.CreateNewTenant(newTenantBo);
                return Ok(newTenantBo.Guid);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.CreateItemFailed, _logger, ex, newTenantApiContract.Name);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Update)]
        [HttpPut("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenant(Guid guid, [FromBody] Common.Models.ApiContracts.Tenant tenantToEditApiContract)
        {
            if (guid != tenantToEditApiContract?.Guid)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemBadData, "Different tenant update attempt.");
                return BadRequest(tenantToEditApiContract);
            }             
            
            try
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Updating tenant with GUID {guid}.", guid);
                
                var tenantToEditBo = _mapper.Map<Tenant>(tenantToEditApiContract);
                await _tenantLogic.EditTenant(tenantToEditBo);
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
                var tenantToDeleteBo = await _tenantLogic.GetByGuid(guid);

                if (tenantToDeleteBo == null)
                {
                    _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Tenant with GUID {guid} for deleting not found.", guid);
                    return NotFound();
                }
                
                await _tenantLogic.DeleteTenant(tenantToDeleteBo.Guid);
                return Ok(tenantToDeleteBo);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.UpdateItemFailed, _logger, ex, guid);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}