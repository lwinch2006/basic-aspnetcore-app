using System;
using System.Collections.Generic;
using Unleash;
using Unleash.Strategies;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public class TenantGuidStrategy : IStrategy
    {
        private const string ParameterName = "tenantGuids";
        
        public string Name => UnleashConstants.TenantGuidStrategyName;
        
        public bool IsEnabled(Dictionary<string, string> parameters, UnleashContext context)
        {
            if (parameters == null || !parameters.ContainsKey(ParameterName) || 
                context?.Properties == null || !context.Properties.ContainsKey(UnleashConstants.TenantGuidStrategyName))
            {
                return false;
            }

            if (parameters[ParameterName] == null &&
                context.Properties[UnleashConstants.TenantGuidStrategyName] == null)
            {
                return true;
            }

            if (parameters[ParameterName] == null ||
                context.Properties[UnleashConstants.TenantGuidStrategyName] == null)
            {
                return false;
            }

            return parameters[ParameterName].Contains(context.Properties[UnleashConstants.TenantGuidStrategyName],
                StringComparison.OrdinalIgnoreCase);
        }
    }
}