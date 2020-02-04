using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration
{
    [Route("Administration/[controller]/{action=Index}")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ILogger<TenantsController> _logger;

        public UsersController(IMapper mapper, ILogger<TenantsController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
        
        [DataOperationAuthorize(nameof(ApplicationUser), DataOperationNames.Read)]
        [HttpGet]
        [ActionName("index")]
        public IActionResult GetAll()
        {
            _logger.LogInformation(LoggingEvents.ReadItems, "Getting all users.");
            
            return Ok();
        }
    }
}