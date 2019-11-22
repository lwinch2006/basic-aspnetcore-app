using System.Collections.Generic;
using Dka.AspNetCore.BasicWebApp.Services.Unleash;
using Unleash;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.UnitTests.Services.Unleash
{
    public class EnvironmentNameStrategyTests
    {
        [Fact]
        public void TestingCreation_ShouldPass()
        {
            var environmentNameStrategy = new EnvironmentNameStrategy();
            Assert.Equal(UnleashConstants.EnvironmentStrategyName, environmentNameStrategy.Name);
        }

        [Fact]
        public void TestingIsEnabledFunction_ParametersNotContainProperKey_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, "development"}
                }
            };

            Assert.False(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }

        [Fact]
        public void TestingIsEnabledFunction_ContextPropertiesNotContainProperKey_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"environmentNames", "development,test"}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"}
                }
            };

            Assert.False(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }        
        
        [Fact]
        public void TestingIsEnabledFunction_ParameterWithProperKeyIsNull_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"environmentNames", null}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, "development"}
                }
            };

            Assert.False(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }
        
        [Fact]
        public void TestingIsEnabledFunction_ContextPropertyIsNull_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"environmentNames", "development,test"}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, null}
                }
            };

            Assert.False(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }        
        
        [Fact]
        public void TestingIsEnabledFunction_ParameterWithProperKeyAndContextPropertyAreNull_ReturnsTrue_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"environmentNames", null}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, null}
                }
            };

            Assert.True(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }         
        
        [Fact]
        public void TestingIsEnabledFunction_EnvironmentIsDifferentFromDefinedOnes_ReturnsFalse_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"environmentNames", "test,production"}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, "development"}
                }
            };

            Assert.False(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }  
        
        [Fact]
        public void TestingIsEnabledFunction_EnvironmentIsAmongDefinedOnes_ReturnsTrue_ShouldPass()
        {
            var parameters = new Dictionary<string, string>
            {
                {"key1", "value1"},
                {"key2", "value2"},
                {"environmentNames", "development,test"}
            };
            
            var environmentNameStrategy = new EnvironmentNameStrategy();
            var unleashContext = new UnleashContext
            {
                Properties = new Dictionary<string, string>
                {
                    {"strategy1", "strategy-value-1"},
                    {UnleashConstants.EnvironmentStrategyName, "development"}
                }
            };

            Assert.True(environmentNameStrategy.IsEnabled(parameters, unleashContext));
        }         
    }
}