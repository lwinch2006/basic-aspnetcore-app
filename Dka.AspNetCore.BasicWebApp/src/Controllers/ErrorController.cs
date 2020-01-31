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
        
        public IActionResult Index()
        {
            var statusCodeException = new WebAppStatusCodeException(HttpContext);
            var errorVm = _mapper.Map<ErrorViewModel>(statusCodeException);
            ExceptionProcessor.ProcessError(LoggingEvents.ApplicationFailed, _logger, HttpContext, statusCodeException, statusCodeException.Path, "Status code: 500");
            
            return View(errorVm);
        }
        
        [Route("Error/500")]
        public IActionResult Error500()
        {
            var statusCodeException = new WebAppStatusCodeException(HttpContext);
            var errorVm = _mapper.Map<ErrorViewModel>(statusCodeException);
            ExceptionProcessor.ProcessError(LoggingEvents.ApplicationFailed, _logger, HttpContext, statusCodeException, statusCodeException.Path, "Status code: 500");
            
            return View(errorVm);
        }
        
        [Route("Error/404")]
        public IActionResult Error404()
        {
            var statusCodeException = new WebAppStatusCodeException(HttpContext);
            var errorVm = _mapper.Map<ErrorViewModel>(statusCodeException);
            ExceptionProcessor.ProcessWarning(LoggingEvents.ApplicationNotFound, _logger, HttpContext, statusCodeException, statusCodeException.Path, "Status code: 404");
            
            return View(errorVm);
        }
        
        [Route("Error/403")]
        public IActionResult Error403()
        {
            return Ok();
        }        
    }
}