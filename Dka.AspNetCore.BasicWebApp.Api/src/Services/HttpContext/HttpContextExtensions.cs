using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Dka.AspNetCore.BasicWebApp.Api.Services.HttpContext
{
    public static class HttpContextExtensions
    {
        public static Guid GetAuthenticatedUserGuid(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var authUserGuidAsString = httpContext.User.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Sub)
                .Value;

            var authUserGuid = Guid.Parse(authUserGuidAsString);

            return authUserGuid;
        }
        
        
        
        
    }
}