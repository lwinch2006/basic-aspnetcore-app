using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;

namespace Dka.AspNetCore.BasicWebApp.Models.ExceptionProcessing
{
    public class AuthenticationException : BasicWebAppException
    {
        public AuthenticationException()
            : base("Invalid signin attempt")
        {}
        
        public AuthenticationException(string message)
            : base(message)
        {}
        
        public AuthenticationException(string message, Exception exception)
            : base(message, exception)
        {}
        
        public AuthenticationException(Exception exception)
            : base("Invalid signin attempt", exception)
        {}        
    }
}