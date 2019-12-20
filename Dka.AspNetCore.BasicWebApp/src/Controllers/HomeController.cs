using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<HomeController> _logger;

        private readonly HttpContext _httpContext;        
        
        public HomeController(IInternalApiClient internalApiClient, IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var pageName = string.Empty;

            try
            {
                pageName = await _internalApiClient.GetPageNameAsync("Home");
            }
            catch (ApiConnectionException ex)
            {
                // Logging exception and showing UI message to the user.
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            ViewData[ViewDataKeys.HtmlPageNameReceivedFromApi] = pageName;



            _httpContext.Response.Headers[HttpHeaders.Authorization] = "Bearer test";
            
            
            return View();
        }
    }
}