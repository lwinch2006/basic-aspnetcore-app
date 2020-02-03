using System;
using System.Diagnostics;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing
{
    public class WebAppStatusCodeException : BasicWebAppException
    {
        public string RequestId { get; set; } = string.Empty;

        public string TraceId { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public WebAppStatusCodeException(HttpContext httpContext = null)
            : base("Web application status code exception", GetOriginalStatusCodeException(httpContext))
        {
            SetupProperties(httpContext);
        }
        
        public WebAppStatusCodeException(string message, HttpContext httpContext = null)
            : base(message, GetOriginalStatusCodeException(httpContext))
        {
            SetupProperties(httpContext);
        }

        private void SetupProperties(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return;
            }
            
            RequestId = httpContext.TraceIdentifier;
            TraceId = (Activity.Current?.Id ?? string.Empty).TrimStart('|').TrimEnd('.');
            
            var statusCodeReExecuteFeature = httpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCodeReExecuteFeature != null)
            {
                Path =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }
        }

        private static Exception GetOriginalStatusCodeException(HttpContext httpContext)
        {
            var exceptionHandlerPathFeature = httpContext?.Features.Get<IExceptionHandlerPathFeature>();
            return exceptionHandlerPathFeature?.Error;
        }
    }
}