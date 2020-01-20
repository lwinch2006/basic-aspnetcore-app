using System.Security.Claims;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization
{
    public class DataOperationAuthorizationHandlerBasedOnRight : AuthorizationHandler<DataOperationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DataOperationRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            var rightName = $"{requirement.Target}_{requirement.Name}";

            if (context.User.HasClaim(ClaimsCustomTypes.Right, rightName))
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}