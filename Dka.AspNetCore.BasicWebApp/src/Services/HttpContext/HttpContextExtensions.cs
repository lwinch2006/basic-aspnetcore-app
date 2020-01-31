using System;
using System.Linq;
using System.Security.Claims;

namespace Dka.AspNetCore.BasicWebApp.Services.HttpContext
{
    public static class HttpContextExtensions
    {
        public static Guid GetAuthenticatedUserGuid(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var authUserGuidAsString = httpContext.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier)
                .Value;
        
            var authUserGuid = Guid.Parse(authUserGuidAsString);
        
            return authUserGuid;
        }        
    }
}