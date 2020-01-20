using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Authorization
{
    public class DataOperationRequirement : OperationAuthorizationRequirement
    {
        public string Target { get; }

        public DataOperationRequirement(string target, string dataOperation)
        {
            Target = target;
            Name = dataOperation;
        }
    }
}