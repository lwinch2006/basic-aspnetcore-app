using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class StatusController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        private readonly ILogger<StatusController> _logger;
        
        public StatusController(IInternalApiClient internalApiClient, ILogger<StatusController> logger)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index()
        {
            throw new Exception("This is test exception");
            
            
            var apiLiveStatusResult = await _internalApiClient.CheckApiLiveStatus();

            ViewData[ViewDataKeys.ApiLiveStatus] = apiLiveStatusResult;
            
            return View();
        }
    }
}