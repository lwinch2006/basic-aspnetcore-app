using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Api.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Account
{
    public class LoginController : Controller
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public LoginController(IOptions<JwtConfiguration> jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration.Value;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationContract authenticationContract)
        {
            return await Task.FromResult(Ok());
        }
    }
}