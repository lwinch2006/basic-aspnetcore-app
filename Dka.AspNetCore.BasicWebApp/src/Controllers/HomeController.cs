using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(IInternalApiClient internalApiClient, ILogger<HomeController> logger)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi] = "Home";
            }
            catch (BasicWebAppException ex)
            {
                // Logging exception and showing UI message to the user.
                ExceptionProcessor.Process(LoggingEvents.ReadItemsFailed, _logger, HttpContext, ex);
            }
            
            return View();
        }
    }
}