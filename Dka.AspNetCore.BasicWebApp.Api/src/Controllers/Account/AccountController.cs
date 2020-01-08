using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Api.Controllers.Administration;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly JwtConfiguration _jwtConfiguration;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<AccountController> _logger;
        
        public AccountController(IOptions<JwtConfiguration> jwtConfiguration, UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _jwtConfiguration = jwtConfiguration.Value;
            _userManager = userManager;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestContract signInRequestContract)
        {
            if (signInRequestContract == null || 
                string.IsNullOrWhiteSpace(signInRequestContract.Username) || 
                string.IsNullOrWhiteSpace(signInRequestContract.Password))
            {
                return BadRequest();
            }

            if (!(await _userManager.FindByNameAsync(signInRequestContract.Username) is { } user))
            {
                return await Task.FromResult(Ok(new SignInResponseContract
                {
                    SignInResult = SignInResult.Failed
                }));
            }

            if (!await _userManager.CheckPasswordAsync(user, signInRequestContract.Password))
            {
                return await Task.FromResult(Ok(new SignInResponseContract
                {
                    SignInResult = SignInResult.Failed
                }));
            }
            
            var tokenExpireAt = DateTime.UtcNow.AddDays(7);
            var userRole = UserRoleNames.Administrator;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Guid.ToString()),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = tokenExpireAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            var signInResponseContract = new SignInResponseContract
            {
                SignInResult = SignInResult.Success,
                AccessToken = token,
                UserRole = userRole,
                ExpireAt = tokenExpireAt
            };
            
            return await Task.FromResult(Ok(signInResponseContract));
        }
        
        public async Task<IActionResult> SignOut()
        {
            return await Task.FromResult(Ok(new SignOutResponseContract { SignOutResult = SignInResult.Success}));
        }
    }
}