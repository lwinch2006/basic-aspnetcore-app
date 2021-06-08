using System;
using System.Collections.Generic;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Utils;
using Dka.AspNetCore.BasicWebApp.Common.Models.Toastr;
using Xunit;

namespace Dka.AspNetCore.BasicWebApp.Common.UnitTests.ModelTests.Toastr
{
    public class ToastrMessageTypesTests
    {
        private readonly Dictionary<int, (string, string, string)> _tenantCategoryTypesAsDictionary =
            new()
            {
                {1, ("Info", "Information", "Information")},
                {2, ("Success", "Success", "Success")},
                {3, ("Warning", "Warning", "Warning")},
                {4, ("Error", "Error", "Error")},
            };        
        
        [Fact]
        public void Test_ShoulsPass()
        {
            Assert.Equal(_tenantCategoryTypesAsDictionary.Keys.Count, Enum.GetNames<ToastrMessageTypes>().Length);

            foreach (var tenantCategory in _tenantCategoryTypesAsDictionary)
            {
                Assert.True(Enum.IsDefined(typeof(ToastrMessageTypes), tenantCategory.Key));
                Assert.Equal(tenantCategory.Value.Item1, Enum.GetName(typeof(ToastrMessageTypes), tenantCategory.Key));
                Assert.Equal(tenantCategory.Value.Item2, ((ToastrMessageTypes) tenantCategory.Key).GetDisplayName());
                Assert.Equal(tenantCategory.Value.Item3, ((ToastrMessageTypes) tenantCategory.Key).GetDescription());
            }                        
        }        
    }
}