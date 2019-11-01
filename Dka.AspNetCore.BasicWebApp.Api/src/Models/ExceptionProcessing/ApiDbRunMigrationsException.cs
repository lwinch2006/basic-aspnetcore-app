using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;

namespace Dka.AspNetCore.BasicWebApp.Api.Models.ExceptionProcessing
{
    public class ApiDbRunMigrationsException : BasicWebAppException
    {
        public ApiDbRunMigrationsException(string message)
            : base(message)
        {}
        
        public ApiDbRunMigrationsException(string message, Exception exception)
            : base(message, exception)
        {}        
    }
}