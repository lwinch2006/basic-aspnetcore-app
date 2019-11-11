using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration
{
    [Route("Administration/[controller]/{action=Index}")]
    public class TenantsController : Controller
    {
        private readonly TenantLogic _tenantLogic;

        private readonly IMapper _mapper;

        public TenantsController(TenantLogic tenantLogic, IMapper mapper)
        {
            _tenantLogic = tenantLogic;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _tenantLogic.GetAll();

            return Ok(tenants);
        }

        [HttpGet("{guid}")]
        [ActionName("details")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            var tenant = await _tenantLogic.GetByGuid(guid);

            if (tenant == null)
            {
                return NotFound();
            }
            
            return Ok(tenant);
        }

        [HttpPost]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant([FromBody] Common.Models.ApiContracts.NewTenant newTenantApiContract)
        {
            try
            {
                var newTenantBo = _mapper.Map<Tenant>(newTenantApiContract);
                newTenantBo.Guid = await _tenantLogic.CreateNewTenant(newTenantBo);
                return Ok(newTenantBo.Guid);
            }
            catch (Exception ex)
            {
                // TODO: implement exception processing.
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{guid}")]
        [ActionName("edit")]
        public async Task<IActionResult> EditTenant(Guid guid, [FromBody] Common.Models.ApiContracts.Tenant tenantToEditApiContract)
        {
            if (guid != tenantToEditApiContract.Guid)
            {
                return BadRequest(tenantToEditApiContract);
            }

            try
            {
                var tenantToEditBo = _mapper.Map<Tenant>(tenantToEditApiContract);
                await _tenantLogic.EditTenant(tenantToEditBo);
                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: implement exception processing.
                // TODO: implement returning not exist response.
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{guid}")]
        [ActionName("delete")]
        public async Task<IActionResult> DeleteTenant(Guid guid)
        {
            try
            {
                var tenantToDeleteBo = await _tenantLogic.GetByGuid(guid);

                if (tenantToDeleteBo == null)
                {
                    return NotFound();
                }
                
                await _tenantLogic.DeleteTenant(tenantToDeleteBo.Guid);
                return Ok(tenantToDeleteBo);
            }
            catch (Exception ex)
            {
                // TODO: implement exception processing.
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}