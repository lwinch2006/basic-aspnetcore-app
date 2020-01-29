using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization
{
    public class DataOperationAuthorizationHandlerForSupport : AuthorizationHandler<DataOperationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DataOperationRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (!context.User.IsInRole(UserRoleNames.Support))
            {
                return Task.CompletedTask;
            }

            if (requirement.Name == DataOperationNames.Read ||
                requirement.Name == DataOperationNames.Update)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}