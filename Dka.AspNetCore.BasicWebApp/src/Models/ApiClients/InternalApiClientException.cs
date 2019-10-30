using System;
using Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing;
using Newtonsoft.Json.Serialization;

namespace Dka.AspNetCore.BasicWebApp.Models.ApiClients
{
    public class InternalApiClientException : Exception, IBasicWebAppException
    {
        public new string Message 
        {
            get { return base.Message; }
        }

        public new object StackTrace {
            get { return base.StackTrace; }
        }

        public InternalApiClientException(string message)
            : base(message)
        {}
        
        public InternalApiClientException(string message, Exception exception)
            : base(message, exception)
        {}
    }
}