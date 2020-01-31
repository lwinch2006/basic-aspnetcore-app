using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Account
{
    [Route("Account/[controller]/{action=Index}")]
    public class ManageController : Controller
    {
        [DataOperationAuthorize(nameof(ApplicationUser), DataOperationNames.Read)]
        [HttpGet]
        [ActionName("index")]
        public IActionResult Profile()
        {
            return Ok();
        }
    }
}