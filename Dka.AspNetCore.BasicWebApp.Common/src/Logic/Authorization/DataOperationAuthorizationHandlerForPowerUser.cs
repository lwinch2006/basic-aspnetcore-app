using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization
{
    public class DataOperationAuthorizationHandlerForPowerUser : AuthorizationHandler<DataOperationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DataOperationRequirement requirement)
        {
            if (!context.User.IsInRole(UserRoleNames.PowerUser))
            {
                return Task.CompletedTask;
            }

            if (requirement.Name == DataOperationNames.Read)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}