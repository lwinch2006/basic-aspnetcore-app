using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Utils
{
    public static class Extensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayName = enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? string.Empty;

            return displayName;
        }
        
        public static string GetDescription(this Enum enumValue)
        {
            var displayName = enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description ?? string.Empty;

            return displayName;
        }        
        
        public static string GetDisplayName<T>(this T source, string propertyName) where T : class
        {
            var displayName = source
                .GetType()
                .GetProperty(propertyName)
                ?.GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? string.Empty;

            return displayName;
        }
    }
}