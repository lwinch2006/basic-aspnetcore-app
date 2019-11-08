using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Controllers.Administration;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;
        
        private readonly ILogger<TenantsController> _logger;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;
        
        public AdministrationController(
            IInternalApiClient internalApiClient, 
            IHttpContextAccessor httpContextAccessor, 
            ILogger<TenantsController> logger,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _mapper = mapper;
        }        
        
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }
    }
}