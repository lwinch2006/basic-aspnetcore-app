using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization
{
    public class DataOperationAuthorizationPolicyProviderForJwt : IAuthorizationPolicyProvider
    {
        private const string PolicyPrefix = "DataOperation";

        private readonly string _authenticationScheme;
        
        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }
        
        public DataOperationAuthorizationPolicyProviderForJwt(string authenticationScheme, IOptions<AuthorizationOptions> options)
        {
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);

            _authenticationScheme = authenticationScheme;
        }
        
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder(_authenticationScheme).RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return BackupPolicyProvider.GetPolicyAsync(policyName);
            }

            var policyParts = policyName.Split("_");

            if (policyParts.Length != 3)
            {
                return BackupPolicyProvider.GetPolicyAsync(policyName);
            }

            var target = policyParts[1];
            var dataOperation = policyParts[2];
            
            var policy = new AuthorizationPolicyBuilder(_authenticationScheme);
            policy.AddRequirements(new DataOperationRequirement(target, dataOperation));
            return Task.FromResult(policy.Build());
        }
    }
}