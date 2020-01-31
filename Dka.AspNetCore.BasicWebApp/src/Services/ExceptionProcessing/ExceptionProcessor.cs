using Dka.AspNetCore.BasicWebApp.Common.Models.Toastr;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Models.ApiClients;
using Microsoft.Extensions.Logging;

namespace Dka.AspNetCore.BasicWebApp.Services.ExceptionProcessing
{
    public static class ExceptionProcessor
    {
        public static void Process(EventId eventId, ILogger logger, Microsoft.AspNetCore.Http.HttpContext httpContext, BasicWebAppException exception, params object[] parameters)
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

            logger.LogError(eventId, exception, messageTemplate, exception.Message, parameters);            
            
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