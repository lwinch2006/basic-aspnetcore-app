using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.ViewModels.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Dka.AspNetCore.BasicWebApp.Services.Pagination
{
    public class PaginationMiddleware
    {
        private readonly RequestDelegate _next;
        
        public PaginationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var pageSizeName = nameof(PaginationRequestViewModel.PageSize);
            var pageSize = context.Request.Query[pageSizeName];
            if (!string.IsNullOrWhiteSpace(pageSize) && int.TryParse(pageSize, out var pageSizeAsInteger))
            {
                var cookieOptions = new CookieOptions
                {
                    Path = "/",
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = false,
                    MaxAge = TimeSpan.FromDays(1)
                };
                
                context.Response.Cookies.Append(pageSizeName, pageSizeAsInteger.ToString(), cookieOptions);
            }

            await _next(context);
        }
    }
    
    public static class PaginationMiddlewareExtensions
    {
        public static IApplicationBuilder UsePaginationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PaginationMiddleware>();
        }
    }
}