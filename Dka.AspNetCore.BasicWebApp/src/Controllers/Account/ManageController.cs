using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Account
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}