using System;
using System.Threading.Tasks;
using Dka.AspNetCore.BasicWebApp.Common.Logic;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Moq;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests
{
    public class EnvironmentLogicTests
    {
        [Fact]
        public void IsDevelopment_EnvironmentIsDevelopment_ReturnsTrue_ShouldPass()
        {
           Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Development");
           
           Assert.True(EnvironmentLogic.IsDevelopment());
        }
        
        [Fact]
        public void IsDevelopment_EnvironmentIsNotDevelopment_ReturnsFalse_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Development111");
           
            Assert.False(EnvironmentLogic.IsDevelopment());
        }
        
        [Fact]
        public void IsDevelopment_EnvironmentIsTest_ReturnsTrue_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Test");
           
            Assert.True(EnvironmentLogic.IsTest());
        }
        
        [Fact]
        public void IsDevelopment_EnvironmentIsNotTest_ReturnsFalse_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Test111");
           
            Assert.False(EnvironmentLogic.IsTest());
        }        
        
        [Fact]
        public void IsDevelopment_EnvironmentIsStage_ReturnsTrue_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Stage");
           
            Assert.True(EnvironmentLogic.IsStage());
        }
        
        [Fact]
        public void IsDevelopment_EnvironmentIsNotStage_ReturnsFalse_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Stage111");
           
            Assert.False(EnvironmentLogic.IsStage());
        }
        
        [Fact]
        public void IsDevelopment_EnvironmentIsProduction_ReturnsTrue_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Production");
           
            Assert.True(EnvironmentLogic.IsProduction());
        }
        
        [Fact]
        public void IsDevelopment_EnvironmentIsNotProduction_ReturnsFalse_ShouldPass()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName, "Production111");
           
            Assert.False(EnvironmentLogic.IsProduction());
        }        
    }
}