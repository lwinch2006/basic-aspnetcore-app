using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;

        public AboutController(IInternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["PageNameFromApi"] = await _internalApiClient.GetPageNameAsync("About");
            
            return View();
        }
    }
}