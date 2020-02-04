using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Administration
{
    public class UsersController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<UsersController> _logger;

        private readonly IMapper _mapper;        
        
        public UsersController(
            IInternalApiClient internalApiClient, 
            ILogger<UsersController> logger,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
            _mapper = mapper;
        }        
        
        [DataOperationAuthorize(nameof(ApplicationUser), DataOperationNames.Read)]
        [HttpGet]
        [ActionName("index")]
        public async Task<IActionResult> GetAll()
        {
            var usersViewModel = (IEnumerable<ApplicationUserViewModel>)new List<ApplicationUserViewModel>();
            
            try
            {
                
                
                
                //var usersContract = await _internalApiClient.GetApplicationUsers();
                
                
                
                
                
                
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, _logger, HttpContext, ex);
            }

            return View("~/Views/Administration/Users/UserList.cshtml", usersViewModel);
        }        
        
        
        
        
        
        

    }
}