using Microsoft.AspNetCore.Authorization;

namespace Dka.AspNetCore.BasicWebApp.Common.Models.Authorization
{
    public class DataOperationAuthorizeAttribute : AuthorizeAttribute
    {
        private const string POLICY_PREFIX = "DataOperation";

        public string Target { get; }

        public string DataOperation { get; }

        public DataOperationAuthorizeAttribute(string target, string dataOperation)
        {
            Target = target;
            DataOperation = dataOperation;
            Policy = $"{POLICY_PREFIX}_{Target}_{DataOperation}";
        }
    }
}