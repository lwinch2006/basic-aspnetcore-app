using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly InternalApiClient _internalApiClient;
        
        public HomeController(InternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
        }

        public async Task<IActionResult> Index()
        {

            ViewData["PageNameFromApi"] = await _internalApiClient.GetPageNameAsync("Home");
            
            return View();
        }
    }
}