using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration;
using Dka.AspNetCore.BasicWebApp.Api.Services.HttpContext;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Account
{
    [Route("Account/[controller]/{action=Index}")]
    public class ManageController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ILogger<TenantsController> _logger;
        
        public ManageController(IMapper mapper, ILogger<TenantsController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
        
        [DataOperationAuthorize(nameof(ApplicationUser), DataOperationNames.Read)]
        [HttpGet]
        [ActionName("index")]
        public IActionResult Profile()
        {
            _logger.LogInformation(LoggingEvents.ReadItems, "Getting user with GUID {Guid}.", HttpContext.GetAuthenticatedUserGuid());
            
            return Ok();
        }
    }
}