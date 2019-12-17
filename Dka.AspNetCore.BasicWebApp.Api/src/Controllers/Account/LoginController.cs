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
            if (authenticationContract == null)
            {
                return BadRequest();
            }


            
            
            
            
            



            var loggedInUser = new LoggedInUserContract
            {
                Guid = new Guid("7ea7c405-5d05-4f72-849d-6c39c011305c")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, loggedInUser.Guid.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            loggedInUser.JwtToken = tokenHandler.WriteToken(token);

            return await Task.FromResult(Ok(loggedInUser));
        }
    }
}