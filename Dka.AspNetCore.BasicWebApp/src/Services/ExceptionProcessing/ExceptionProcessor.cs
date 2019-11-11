using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.Toastr;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Dka.AspNetCore.BasicWebApp.Models.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing
{
    public static class ExceptionProcessor
    {
        public static void Process(ILogger logger, HttpContext httpContext, BasicWebAppException exception)
        {
            // Logging.
            logger.LogError("{0}{1}{2}", exception.Message, Environment.NewLine, exception.StackTrace);
            
            // UI Message.
            httpContext.Items[ToastrConstants.MessageType] = ToastrMessageTypes.Error.ToString().ToLower();
            
            switch (exception)
            {
                case ApiConnectionException _:
                    httpContext.Items[ToastrConstants.Message] = UserFriendlyErrorMessageConstants.ApiConnectionException;                       
                    break;
                
                case ApiStatusCodeException _:
                    httpContext.Items[ToastrConstants.Message] = UserFriendlyErrorMessageConstants.ApiStatusCodeException;  
                    break;
                
                case TenantNotFoundException _:
                    httpContext.Items[ToastrConstants.Message] = UserFriendlyErrorMessageConstants.TenantNotFoundException;                       
                    break;
                
                default:
                    httpContext.Items[ToastrConstants.Message] = UserFriendlyErrorMessageConstants.GeneralException;                     
                    break;
            }
        }
    }
}