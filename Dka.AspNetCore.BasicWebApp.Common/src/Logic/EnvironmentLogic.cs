using System;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic
{
    public static class EnvironmentLogic
    {
        private const string DevelopmentEnvironmentName = "Development";
        private const string StageEnvironmentName = "Stage";
        private const string TestEnvironmentName = "Test";
        private const string ProductionEnvironmentName = "Production";
        private const string DefaultAspNetCoreEnvironment = ProductionEnvironmentName;
        private const string AspNetCoreEnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";
        
        public static bool IsDevelopment()
        {
            var environment = Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableName) ?? DefaultAspNetCoreEnvironment;
            return environment.Equals(DevelopmentEnvironmentName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsTest()
        {
            var environment = Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableName) ?? DefaultAspNetCoreEnvironment;
            return environment.Equals(TestEnvironmentName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsStage()
        {
            var environment = Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableName) ?? DefaultAspNetCoreEnvironment;
            return environment.Equals(StageEnvironmentName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsProduction()
        {
            var environment = Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableName) ?? DefaultAspNetCoreEnvironment;
            return environment.Equals(ProductionEnvironmentName, StringComparison.OrdinalIgnoreCase);
        }
    }
}