using System;
using System.Linq;
using System.Reflection;
using Dka.AspNetCore.BasicWebApp.Common.Models.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomMappers(this IServiceCollection services, params Type[] types)
        {
            var baseType = typeof(IBaseCustomMapper);

            foreach (var type in types)
            {
                var assembly = type.GetTypeInfo().Assembly;

                var childTypes = assembly.GetTypes().Where(tempType => baseType.IsAssignableFrom(tempType));
                var childInterfaces = childTypes.Where(tempType => tempType.IsInterface && tempType != baseType);

                foreach (var childInterface in childInterfaces)
                {
                    var childClasses = childTypes.Where(tempType => tempType.IsClass && childInterface.IsAssignableFrom(tempType));

                    foreach (var childClass in childClasses)
                    {
                        services.AddScoped(childInterface, childClass);
                    }
                }                
            }
        }        
    }
}