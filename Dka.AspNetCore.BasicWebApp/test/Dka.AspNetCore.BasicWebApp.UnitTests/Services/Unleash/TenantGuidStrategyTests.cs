using System.Collections.Generic;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Unleash;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class TenantGuidStrategyTests
    {
        [Fact]
        public void TestingCreation_ShouldPass()
        {
            var tenantGuidStrategy = new TenantGuidStrategy();
            Assert.Equal(UnleashConstants.TenantGuidStrategyName, tenantGuidStrategy.Name);
        }

        [Fact]
        public void TestingIsEnabledFunction_ParametersNotContainProperKey_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.TenantGuidStrategyName, "8C01ADB2-2E2E-4A88-BF2E-0E262DB3361D"}
                }
            };

            Assert.False(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }

        [Fact]
        public void TestingIsEnabledFunction_ContextPropertiesNotContainProperKey_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"tenantGuids", "24F170EB-9B38-4CDD-8CF5-E3190900ABDE,7029C640-6D22-4214-B4A3-D98485EDC15C"}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"}
                }
            };

            Assert.False(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }        
        
        [Fact]
        public void TestingIsEnabledFunction_ParameterWithProperKeyIsNull_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"tenantGuids", null}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.TenantGuidStrategyName, "7029C640-6D22-4214-B4A3-D98485EDC15C"}
                }
            };

            Assert.False(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }
        
        [Fact]
        public void TestingIsEnabledFunction_ContextPropertyIsNull_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"tenantGuids", "DACCE154-6CD7-4325-9A8C-D4D56D063283,03CA2179-B442-426D-834B-B7BF3998D13D"}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.TenantGuidStrategyName, null}
                }
            };

            Assert.False(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }        
        
        [Fact]
        public void TestingIsEnabledFunction_ParameterWithProperKeyAndContextPropertyAreNull_ReturnsTrue_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"tenantGuids", null}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.TenantGuidStrategyName, null}
                }
            };

            Assert.True(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }         
        
        [Fact]
        public void TestingIsEnabledFunction_TenantIsDifferentFromDefinedOnes_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"tenantGuids", "D0B15898-85A0-4264-BBF6-EDFFB31E0891,486879A9-0895-493C-AA18-F9586C99384C"}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, "D13E9E29-2A5C-4C93-8954-B525E112581D"}
                }
            };

            Assert.False(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }  
        
        [Fact]
        public void TestingIsEnabledFunction_TenantIsAmongDefinedOnes_ReturnsTrue_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"tenantGuids", "05A4D41A-B207-4EB9-88D8-AF5C910DAAE5,FB447B6E-FF4B-4AAC-82A5-C7CDCE4028E4"}
            };
            
            var tenantGuidStrategy = new TenantGuidStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.TenantGuidStrategyName, "05A4D41A-B207-4EB9-88D8-AF5C910DAAE5"}
                }
            };

            Assert.True(tenantGuidStrategy.IsEnabled(parameters, unleashContext));
        }             
    }
}