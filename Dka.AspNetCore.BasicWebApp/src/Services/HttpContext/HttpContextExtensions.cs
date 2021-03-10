using System;
using System.Linq;
using System.Security.Claims;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;

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
        
        public static int? TryGetPageSizeFromCookie(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var pageSizeAsString = httpContext.Request.Cookies[nameof(PaginationRequestViewModel.PageSize)];

            if (pageSizeAsString == null)
            {
                return null;
            }

            var pageSizeAsInteger = int.Parse(pageSizeAsString);
            return pageSizeAsInteger;
        }
    }
}