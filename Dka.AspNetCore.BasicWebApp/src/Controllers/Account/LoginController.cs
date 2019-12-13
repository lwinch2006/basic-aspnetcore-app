using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Account
{
    public class LoginController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}