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
        
        private readonly ILogger<AdministrationController> _logger;

        private readonly IMapper _mapper;
        
        public AdministrationController(
            IInternalApiClient internalApiClient, 
            ILogger<AdministrationController> logger,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
            _mapper = mapper;
        }        
        
        public IActionResult Index()
        {
            return View();
        }
    }
}