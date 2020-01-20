using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration
{
    public class UserController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}