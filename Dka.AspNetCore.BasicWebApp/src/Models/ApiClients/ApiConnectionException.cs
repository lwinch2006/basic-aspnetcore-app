using System;
using System.Security.Cryptography;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Newtonsoft.Json.Serialization;

namespace Dka.AspNetCore.BasicWebApp.Models.ApiClients
{
    public class ApiConnectionException : BasicWebAppException
    {
        public ApiConnectionException()
            : base("API connection exception")
        {}

        public ApiConnectionException(string message)
            : base(message)
        {}
        
        public ApiConnectionException(string message, Exception exception)
            : base(message, exception)
        {}
        
        public ApiConnectionException(Exception exception)
            : base("API connection exception", exception)
        {}
    }
}