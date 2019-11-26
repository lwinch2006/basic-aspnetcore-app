using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Unleash;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public class UnleashMiddleware
    {
        private readonly RequestDelegate _next;

        public UnleashMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostEnvironment env)
        {
            var sessionId = string.Empty;

            try
            {
                sessionId = context.Session.Id;
            }
            catch (InvalidOperationException)
            {
                // TODO: Process exception in some way.
            }
            
            context.Items["UnleashContext"] = new UnleashContext
            {
                UserId = context.User?.Identity?.Name,
                SessionId = sessionId,
                RemoteAddress = context.Request.Host.Host,
                Properties = new Dictionary<string, string>
                {
                    { UnleashConstants.EnvironmentStrategyName, env.EnvironmentName },
                    { UnleashConstants.TenantGuidStrategyName, "faeadb60-75bf-4b63-a1f7-84e2fc5d681c" }
                }
            };            
            
            await _next.Invoke(context);
        }        
    }
}