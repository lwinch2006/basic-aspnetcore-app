using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers
{
    public class PagesController : ControllerBase
    {
        [Authorize]
        [ActionName("GetPageName")]
        public async Task<IActionResult> GetPageName([FromQuery] string pageName)
        {
            return Ok(pageName);
        }
    }
}