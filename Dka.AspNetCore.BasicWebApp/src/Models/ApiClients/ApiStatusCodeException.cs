using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;

namespace Dka.AspNetCore.BasicWebApp.Models.ApiClients
{
    public class ApiStatusCodeException : BasicWebAppException
    {
        public ApiStatusCodeException(string message)
            : base(message)
        {}
        
        public ApiStatusCodeException(string message, Exception exception)
            : base(message, exception)
        {}
        
        public ApiStatusCodeException(Exception exception)
            : base("API status code exception", exception)
        {}        
    }
}