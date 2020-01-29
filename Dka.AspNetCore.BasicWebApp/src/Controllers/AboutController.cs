using System.Threading.Tasks;
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
    [Authorize(AuthenticationSchemes=CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AboutController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;

        private readonly ILogger<AboutController> _logger;

        private readonly HttpContext _httpContext;

        public AboutController(IInternalApiClient internalApiClient, IHttpContextAccessor httpContextAccessor, ILogger<AboutController> logger)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi] = "About";
            }
            catch (ApiConnectionException ex)
            {
                // Logging exception and showing UI message to the user.
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View();
        }
    }
}