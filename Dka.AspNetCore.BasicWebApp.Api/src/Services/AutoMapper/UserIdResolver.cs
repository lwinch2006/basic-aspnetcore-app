using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Services.HttpContext;
using Dka.AspNetCore.BasicWebApp.Common.Models.ApiContracts.Tenants;
using Dka.AspNetCore.BasicWebApp.Common.Models.Tenants;
using Microsoft.AspNetCore.Http;

namespace Dka.AspNetCore.BasicWebApp.Api.Services.AutoMapper
{
    public class UserIdResolver : IValueResolver<NewTenantContract, Tenant, int>
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;
        
        public UserIdResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public int Resolve(NewTenantContract source, Tenant destination, int destMember, ResolutionContext context)
        {
            return _httpContext.GetUserId();
        }
    }
}