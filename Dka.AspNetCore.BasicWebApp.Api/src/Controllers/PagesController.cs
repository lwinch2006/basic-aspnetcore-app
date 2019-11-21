using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers
{
    public class PagesController : Controller
    {
        public async Task<IActionResult> GetPageName([FromQuery] string pageName)
        {
            return Ok(pageName);
        }
    }
}