using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.Toastr;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Models.Logging;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing
{
    public static class ExceptionProcessor
    {
        public static void ProcessWarning(EventId eventId, ILogger logger, Microsoft.AspNetCore.Http.HttpContext httpContext, Exception exception, params object[] parameters)
        {
            Process(LoggingLevelTypes.Warning, eventId, logger, httpContext, exception, parameters);
        }

        public static void ProcessError(EventId eventId, ILogger logger, Microsoft.AspNetCore.Http.HttpContext httpContext, Exception exception, params object[] parameters)
        {
            Process(LoggingLevelTypes.Error, eventId, logger, httpContext, exception, parameters);
        }
        
        public static void Process(LoggingLevelTypes errorLevel, EventId eventId, ILogger logger, Microsoft.AspNetCore.Http.HttpContext httpContext, Exception exception, params object[] parameters)
        {
            // Logging.
            var messageTemplate = "{Message}";

            if (parameters?.Length > 0)
            {
                messageTemplate = "{Message}. Parameters(";
                
                for (var i = 0; i < parameters.Length - 1; i++)
                {
                    messageTemplate += $"p{i + 1}={{p{i + 1}}}, ";
                }

                messageTemplate += $"p{parameters.Length}={{p{parameters.Length}}})";
            }

            switch (errorLevel)
            {
                case LoggingLevelTypes.Information:
                    logger.LogInformation(eventId, exception, messageTemplate, exception.Message, parameters); 
                    break;
                
                case LoggingLevelTypes.Warning:
                    logger.LogWarning(eventId, exception, messageTemplate, exception.Message, parameters); 
                    break;
                
                case LoggingLevelTypes.Error:
                    logger.LogError(eventId, exception, messageTemplate, exception.Message, parameters); 
                    break;
                
                default:
                    logger.LogError(eventId, exception, messageTemplate, exception.Message, parameters);
                    break;
            }
            
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
                
                default:
                    httpContext.Items[ToastrConstants.Message] = UserFriendlyErrorMessageConstants.GeneralException;                     
                    break;
            }
        }
    }
}