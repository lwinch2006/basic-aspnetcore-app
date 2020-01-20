using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Account
{
    [Authorize]
    public class ManageController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}