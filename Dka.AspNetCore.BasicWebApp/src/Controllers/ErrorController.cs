using System;
using System.Diagnostics;
using System.IO;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.ViewModels.ExceptionProcessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        private readonly IMapper _mapper;
        
        public ErrorController(ILogger<ErrorController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [Route("Error/{errorCode}")]
        public IActionResult ErrorWithCode(int errorCode)
        {
            var statusCodeException = new WebAppStatusCodeException(HttpContext);
            var errorVm = _mapper.Map<ErrorViewModel>(statusCodeException);
            var eventCode = int.Parse($"{errorCode}{LoggingEventPostfixes.Application}");
            
            ExceptionProcessor.ProcessWarning(eventCode, _logger, HttpContext, statusCodeException, statusCodeException.Path, $"Status code: {errorCode}");
            
            return View(errorVm);
        }
    }
}