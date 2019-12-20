using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Account
{
    [Authorize]
    [Route("Account/[controller]/{action=Index}")]
    public class LoginController : Controller
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public LoginController(IOptions<JwtConfiguration> jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration.Value;
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestContract signInRequestContract)
        {
            if (signInRequestContract == null)
            {
                return BadRequest();
            }
            
            var tokenExpireAt = DateTime.UtcNow.AddDays(7);
            var userRole = "Administrator";
            var userId = new Guid("7ea7c405-5d05-4f72-849d-6c39c011305c");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(ClaimTypes.Role, userRole),
                    
                }),
                Expires = tokenExpireAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            var signInResponseContract = new SignInResponseContract
            {
                SignInResult = SignInResult.Success,
                AccessToken = $"Bearer {token}",
                UserRole = userRole,
                ExpireAt = tokenExpireAt
            };
            
            return await Task.FromResult(Ok(signInResponseContract));
        }
        
        [ActionName("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            return await Task.FromResult(Ok(new SignOutResponseContract { SignOutResult = SignInResult.Success}));
        }
    }
}