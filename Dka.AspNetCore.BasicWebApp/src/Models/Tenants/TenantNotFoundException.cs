using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;

namespace Dka.AspNetCore.BasicWebApp.Models.Tenants
{
    public class TenantNotFoundException : BasicWebAppException
    {
        public TenantNotFoundException()
            : base("Tenant not found exception")
        {}
        
        public TenantNotFoundException(string message)
            : base(message)
        {}
        
        public TenantNotFoundException(string message, Exception exception)
            : base(message, exception)
        {}
        
        public TenantNotFoundException(Exception exception)
            : base("Tenant not found exception", exception)
        {}
    }
}