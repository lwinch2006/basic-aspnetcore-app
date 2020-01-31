using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Services.HttpContext;
using Dka.AspNetCore.BasicWebApp.Services.ModelState;
using Dka.AspNetCore.BasicWebApp.ViewModels.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Dka.AspNetCore.BasicWebApp.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;

        private readonly ILogger<AccountController> _logger;

        private readonly IMapper _mapper;

        public AccountController(IInternalApiClient internalApiClient, ILogger<AccountController> logger, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            returnUrl ??= "/";
            
            ViewData[ViewDataNames.ReturnUrl] = returnUrl;
            
            return await Task.FromResult(View("~/Views/Account/SignIn.cshtml"));
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string returnUrl = null)
        {
            ViewData[ViewDataNames.ReturnUrl] = returnUrl;
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.SignInUserBadData, "SignIn user model is not valid. {ErrorMessages}", ModelState.GetModelStateErrorMessages());
                
                return View("~/Views/Account/SignIn.cshtml", signInViewModel);
            }            

            try
            {
                var signInRequestContract = _mapper.Map<SignInRequestContract>(signInViewModel);

                var signInResponseContract = await _internalApiClient.SignIn(signInRequestContract);
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, signInResponseContract.UserGuid.ToString()),
                    new Claim(ClaimTypes.Name, signInViewModel.Username),
                    new Claim(ClaimsCustomTypes.AccessToken, signInResponseContract.AccessToken)
                };

                foreach (var userRole in signInResponseContract.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authOptions = new AuthenticationProperties();
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    authOptions);
                
                _logger.LogInformation(LoggingEvents.SignInUser, "Signing in user with GUID {Guid}.", signInResponseContract.UserGuid.ToString());
                
                return LocalRedirect(returnUrl);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(LoggingEvents.SignInUserFailed, _logger, HttpContext, ex, signInViewModel.Username);
            }

            return View("~/Views/Account/SignIn.cshtml", signInViewModel);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            _logger.LogInformation(LoggingEvents.SignOutUser, "Signing out user with GUID {Guid}.", HttpContext.GetAuthenticatedUserGuid());
            
            return LocalRedirect("~/");
        }
    }
}