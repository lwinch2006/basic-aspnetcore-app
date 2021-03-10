using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;

namespace Dka.AspNetCore.BasicWebApp.Api.Services.HttpContext
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserGuid(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var authUserGuidAsString = httpContext.User.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Sub)
                .Value;

            var authUserGuid = Guid.Parse(authUserGuidAsString);

            return authUserGuid;
        }
        
        public static int GetUserId(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var userIdAsString = httpContext.User.Claims.Single(claim => claim.Type == ClaimsCustomTypes.UserId)
                .Value;

            var userId = int.Parse(userIdAsString);

            return userId;
        }
    }
}