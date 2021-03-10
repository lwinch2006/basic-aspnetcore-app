using System;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Pagination;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Common.Models.Pagination;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration
{
    [ApiController]
    [Route("Administration/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantLogic _tenantLogic;

        private readonly IMapper _mapper;

        private readonly ILogger<TenantsController> _logger;

        public TenantsController(ITenantLogic tenantLogic, IMapper mapper, ILogger<TenantsController> logger)
        {
            _tenantLogic = tenantLogic;
            _mapper = mapper;
            _logger = logger;
        }
        
        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequestContract paginationRequestContract = null)
        {
            _logger.LogInformation(LoggingEvents.ReadItems, "Getting all tenants");

            var pagination = _mapper.Map<Pagination>(paginationRequestContract);
            var tenants = await _tenantLogic.Get(pagination);
            var tenantsContract = _mapper.Map<PagedResults<TenantContract>>(tenants);
            
            return Ok(tenantsContract);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Read)]
        [HttpGet("{guid}")]
        public async Task<IActionResult> Details(Guid guid)
        {
            _logger.LogInformation(LoggingEvents.ReadItem, "Getting tenant with GUID {Guid}", guid);
            
            var tenant = await _tenantLogic.Get(guid);

            if (tenant == null)
            {
                _logger.LogWarning(LoggingEvents.ReadItemNotFound, "Tenant with GUID {Guid} not found", guid);
                return NotFound();
            }
            
            var tenantContract = _mapper.Map<TenantContract>(tenant);
            
            return Ok(tenantContract);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Create)]
        [HttpPost]
        public async Task<IActionResult> Create(NewTenantContract newTenantContract)
        {
            if (newTenantContract == null)
            {
                _logger.LogWarning(LoggingEvents.CreateItemBadData, "Empty tenant cannot be created");
                return BadRequest();
            }
            
            try
            {
                _logger.LogInformation(LoggingEvents.CreateItem, "Creating new tenant with name {Name}", newTenantContract.Name);

                var newTenant = _mapper.Map<Tenant>(newTenantContract);
                newTenant.Guid = await _tenantLogic.Create(newTenant);
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
        public async Task<IActionResult> Update(Guid guid, EditTenantContract editTenantContract)
        {
            try
            {
                var tenantToEdit = await _tenantLogic.Get(guid);

                if (tenantToEdit == null)
                {
                    _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Tenant with GUID {Guid} for updating not found", guid);
                    return NotFound();
                }                
                
                _logger.LogInformation(LoggingEvents.UpdateItem, "Updating tenant with GUID {Guid}", guid);
                
                _mapper.Map(editTenantContract, tenantToEdit);
                await _tenantLogic.Update(tenantToEdit);
                return NoContent();
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.UpdateItemFailed, _logger, ex, guid);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [DataOperationAuthorize(nameof(Tenant), DataOperationNames.Delete)]
        [HttpDelete("{guid}")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            _logger.LogInformation(LoggingEvents.ReadItem, "Deleting tenant with GUID {Guid}", guid);
            
            try
            {
                var tenantToDelete = await _tenantLogic.Get(guid);

                if (tenantToDelete == null)
                {
                    _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Tenant with GUID {Guid} for deleting not found", guid);
                    return NotFound();
                }
                
                await _tenantLogic.Delete(tenantToDelete.Guid);
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