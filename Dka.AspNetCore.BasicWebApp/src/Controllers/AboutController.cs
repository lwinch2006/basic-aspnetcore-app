using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;

        private readonly ILogger<AboutController> _logger;

        public AboutController(IInternalApiClient internalApiClient, ILogger<AboutController> logger)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi] = "About";
            }
            catch (ApiConnectionException ex)
            {
                // Logging exception and showing UI message to the user.
                ExceptionProcessor.ProcessError(LoggingEvents.ReadItemsFailed, _logger, HttpContext, ex);
            }

            return View();
        }
    }
}