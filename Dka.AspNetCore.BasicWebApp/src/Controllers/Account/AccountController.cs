using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Services.ApiClients;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
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

        private readonly HttpContext _httpContext;
        
        private readonly IMapper _mapper;

        public AccountController(IInternalApiClient internalApiClient, IHttpContextAccessor httpContextAccessor, ILogger<AccountController> logger, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            returnUrl ??= "/";
            
            ViewData[ViewDataNames.ReturnUrl] = returnUrl;
            
            return await Task.FromResult(View("~/Views/Account/SignIn.cshtml"));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string returnUrl = null)
        {
            ViewData[ViewDataNames.ReturnUrl] = returnUrl;
            
            if (!ModelState.IsValid)
            {
                return View("~/Views/Account/SignIn.cshtml", signInViewModel);
            }            

            try
            {
                var signInRequestContract = _mapper.Map<SignInRequestContract>(signInViewModel);

                var signInResponseContract = await _internalApiClient.SignIn(signInRequestContract);

                if (signInResponseContract.SignInResult != SignInResult.Success)
                {
                    throw new AuthenticationException();
                }
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, signInViewModel.Username),
                    new Claim(ClaimTypes.Role, signInResponseContract.UserRole),
                    new Claim(ClaimsCustomTypes.AccessToken, signInResponseContract.AccessToken)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authOptions = new AuthenticationProperties();
            
                await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                    authOptions);
                
                return LocalRedirect(returnUrl);
            }
            catch (BasicWebAppException ex)
            {
                ExceptionProcessor.Process(_logger, _httpContext, ex);
            }

            return View("~/Views/Account/SignIn.cshtml", signInViewModel);
        }

        public async Task SignOut()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}