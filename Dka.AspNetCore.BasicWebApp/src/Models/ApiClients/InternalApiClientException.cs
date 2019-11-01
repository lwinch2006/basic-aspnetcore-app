using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Newtonsoft.Json.Serialization;

namespace Dka.AspNetCore.BasicWebApp.Models.ApiClients
{
    public class InternalApiClientException : BasicWebAppException
    {
        public InternalApiClientException(string message)
            : base(message)
        {}
        
        public InternalApiClientException(string message, Exception exception)
            : base(message, exception)
        {}
    }
}