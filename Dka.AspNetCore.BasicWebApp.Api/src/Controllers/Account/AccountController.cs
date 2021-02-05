using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Api.Services.HttpContext;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dka.AspNetCore.BasicWebApp.Api.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly JwtConfiguration _jwtConfiguration;

        private readonly UserManager<ApplicationUser> _userManager;
        
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ILogger<AccountController> _logger;
        
        public AccountController(IOptions<JwtConfiguration> jwtConfiguration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AccountController> logger)
        {
            _jwtConfiguration = jwtConfiguration.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestContract signInRequestContract)
        {
            if (signInRequestContract == null || 
                string.IsNullOrWhiteSpace(signInRequestContract.Username) || 
                string.IsNullOrWhiteSpace(signInRequestContract.Password))
            {
                _logger.LogWarning(LoggingEvents.SignInUserBadData, "Empty username or password not allowed.");
                return BadRequest();
            }

            if (!(await _userManager.FindByNameAsync(signInRequestContract.Username) is { } user))
            {
                _logger.LogWarning(LoggingEvents.SignInUserNotFound, "User with username {Username} not found.", signInRequestContract.Username);
                return NotFound();
            }
            
            if (!await _userManager.CheckPasswordAsync(user, signInRequestContract.Password))
            {
                _logger.LogWarning(LoggingEvents.SignInUserNotFound, "User with username {Username} provided password not valid.", signInRequestContract.Username);
                return NotFound();
            }
            
            _logger.LogInformation(LoggingEvents.SignInUser, "Signing in user with GUID {Guid}.", user.Guid);
            
            var tokenExpireAt = DateTime.UtcNow.AddDays(7);

            var userRoles= await _userManager.GetRolesAsync(user) ?? new List<string>();
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Guid.ToString())
                }),
                Expires = tokenExpireAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (var userRole in userRoles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userRole));
            }
            
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            var signInResponseContract = new SignInResponseContract
            {
                AccessToken = token,
                UserGuid = user.Guid,
                UserRoles = userRoles,
                ExpireAt = tokenExpireAt
            };
            
            return await Task.FromResult(Ok(signInResponseContract));
        }
        
        public new async Task<IActionResult> SignOut()
        {
            _logger.LogInformation(LoggingEvents.SignOutUser, "Signing out user with GUID {Guid}.", HttpContext.GetAuthenticatedUserGuid());
            
            return await Task.FromResult(Ok(new SignOutResponseContract()));
        }
    }
}