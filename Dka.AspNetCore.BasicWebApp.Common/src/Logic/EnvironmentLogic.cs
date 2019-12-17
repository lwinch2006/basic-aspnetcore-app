using System;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic
{
    public static class EnvironmentLogic
    {
        public static bool IsDevelopment()
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName) ?? WebHostEnvironments.Default;
            return environment.Equals(WebHostEnvironments.Development, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsTest()
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName) ?? WebHostEnvironments.Default;
            return environment.Equals(WebHostEnvironments.Test, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsStage()
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName) ?? WebHostEnvironments.Default;
            return environment.Equals(WebHostEnvironments.Stage, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsProduction()
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName) ?? WebHostEnvironments.Default;
            return environment.Equals(WebHostEnvironments.Production, StringComparison.OrdinalIgnoreCase);
        }
    }
}