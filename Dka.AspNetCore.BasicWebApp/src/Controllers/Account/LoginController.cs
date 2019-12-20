using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
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
    [Authorize]
    [Route("Account/[controller]/{action=Index}")]
    public class LoginController : Controller
    {
        private readonly IInternalApiClient _internalApiClient;

        private readonly ILogger<LoginController> _logger;

        private readonly HttpContext _httpContext;
        
        private readonly IMapper _mapper;

        public LoginController(IInternalApiClient internalApiClient, IHttpContextAccessor httpContextAccessor, ILogger<LoginController> logger, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("SignIn")]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            var signInViewModel = new SignInViewModel
            {
                ReturnUrl = returnUrl
            };

            ViewData["ReturnUrl"] = returnUrl;
            
            return await Task.FromResult(View("~/Views/Account/Login/SignIn.cshtml", signInViewModel));
        }

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        [ActionName("SignIn")]
        public async Task<IActionResult> SignIn([Bind("Username", "Password", "ReturnUrl")] SignInViewModel signInViewModel /*[FromQuery(Name = "returnUrl")] string returnUrl = null*/)
        {
            var returnUrl = signInViewModel.ReturnUrl;
            
            // var signInViewModel = new SignInViewModel
            // {
            //     Username = username,
            //     Password = password,
            //     ReturnUrl = returnUrl
            // };
            
            if (!ModelState.IsValid || signInViewModel == null)
            {
                foreach (var modelState in ViewData.ModelState.Values) {
                    foreach (var error in modelState.Errors)
                    {
                        var msg = error.ErrorMessage;
                    }
                }
                
                return View("~/Views/Account/Login/SignIn.cshtml", signInViewModel);
            }            
            
            returnUrl ??= "/";

            try
            {
                var signInRequestContract = _mapper.Map<SignInRequestContract>(signInViewModel);

                var signInResponseContract = await _internalApiClient.Login(signInRequestContract);

                if (signInResponseContract.SignInResult != SignInResult.Success)
                {
                    // TODO: throw authentication exception here.
                }
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, signInViewModel.Username),
                    new Claim(ClaimTypes.Role, signInResponseContract.UserRole),
                    new Claim(CookieNames.AccessToken, signInResponseContract.AccessToken),
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

            return View("~/Views/Account/Login/SignIn.cshtml", signInViewModel);
        }

        [ActionName("SignOut")]
        public async Task SignOut()
        {
            await _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}