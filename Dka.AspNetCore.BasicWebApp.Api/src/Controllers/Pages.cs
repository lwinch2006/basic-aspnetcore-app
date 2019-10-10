using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers
{
    public class Pages : Controller
    {
        public IActionResult GetPageName([FromQuery] string pageName)
        {
            return Ok(pageName);
        }
    }
}