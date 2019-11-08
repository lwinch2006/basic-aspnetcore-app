using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;

namespace Dka.AspNetCore.BasicWebApp.Models.Tenants
{
    public class TenantNotFoundException : BasicWebAppException
    {
        public TenantNotFoundException(string message)
            : base(message)
        {}
        
        public TenantNotFoundException(string message, Exception exception)
            : base(message, exception)
        {}
    }
}