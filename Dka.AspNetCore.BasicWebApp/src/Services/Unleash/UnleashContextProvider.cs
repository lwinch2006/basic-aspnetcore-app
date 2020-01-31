using Microsoft.AspNetCore.Http;
using Unleash;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public class UnleashContextProvider : IUnleashContextProvider
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;
        
        public UnleashContext Context => _httpContext?.Items["UnleashContext"] as UnleashContext;

        public UnleashContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
    }
}