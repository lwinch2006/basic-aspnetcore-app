using System;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing
{
    public class BasicWebAppException : Exception
    {
        public BasicWebAppException()
            : base("Basic exception")
        {}
        
        public BasicWebAppException(string message)
            : base(message)
        {}
        
        public BasicWebAppException(string message, Exception exception)
            : base(message, exception)
        {}
        
        public BasicWebAppException(Exception exception)
            : base("Basic exception", exception)
        {}           
    }
}