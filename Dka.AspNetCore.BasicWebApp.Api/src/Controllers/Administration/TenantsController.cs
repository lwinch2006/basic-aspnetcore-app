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
            
            return Ok(tenant);
        }

        [HttpPost]
        [ActionName("new")]
        public async Task<IActionResult> CreateNewTenant([FromBody] Common.Models.ApiContracts.NewTenant newTenantApiContract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(newTenantApiContract);
            }

            try
            {
                var newTenantBo = _mapper.Map<Tenant>(newTenantApiContract);
                newTenantBo.Guid = await _tenantLogic.CreateNewTenant(newTenantBo);
                return Ok(newTenantBo.Guid);
            }
            catch (Exception ex)
            {
                
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}