using System;
using System.Collections.Generic;
using Unleash;
using Unleash.Strategies;

namespace Dka.AspNetCore.BasicWebApp.Services.Unleash
{
    public class EnvironmentNameStrategy : IStrategy
    {
        private const string ParameterName = "environmentNames";
        
        public string Name => UnleashConstants.EnvironmentStrategyName;

        public bool IsEnabled(Dictionary<string, string> parameters, UnleashContext context)
        {
            if (parameters == null || !parameters.ContainsKey(ParameterName) || 
                context?.Properties == null || !context.Properties.ContainsKey(UnleashConstants.EnvironmentStrategyName))
            {
                return false;
            }

            if (parameters[ParameterName] == null &&
                context.Properties[UnleashConstants.EnvironmentStrategyName] == null)
            {
                return true;
            }

            if (parameters[ParameterName] == null ||
                context.Properties[UnleashConstants.EnvironmentStrategyName] == null)
            {
                return false;
            }

            return parameters[ParameterName].Contains(context.Properties[UnleashConstants.EnvironmentStrategyName],
                StringComparison.OrdinalIgnoreCase);
        }
    }
}