using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Toastr;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Api.Services.ExceptionProcessing
{
    public static class ExceptionProcessor
    {
        public static void Process(ILogger logger, BasicWebAppException exception)
        {
            // Logging.
            logger.LogError("{0}{1}{2}", exception.Message, Environment.NewLine, exception.StackTrace);
        }        
    }
}