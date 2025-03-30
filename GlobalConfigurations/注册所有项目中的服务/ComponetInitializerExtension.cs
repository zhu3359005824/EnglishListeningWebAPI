using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GlobalConfigurations.注册所有项目中的服务
{
    public static class ComponetInitializerExtension
    {/// <summary>
     /// 向容器中注册所有实现了IComponetInitializer接口的服务
     /// </summary>
     /// <param name="services"></param>
     /// <param name="assemblies"></param>
     /// <returns></returns>
     /// <exception cref="ApplicationException"></exception>

        public static IServiceCollection
            RunComponentInitializer(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();

                var componentInitializerType =
                    types.Where(t => !t.IsAbstract && typeof(IComponetInitializer).IsAssignableFrom(t));

                foreach (var implType in componentInitializerType)
                {
                    var initializer = (IComponetInitializer?)Activator.CreateInstance(implType);

                    if (initializer == null)
                    {
                        throw new ApplicationException($"不能创建{implType}");
                    }
                    initializer.Initialize(services);
                }
            }
            return services;
        }
    }
}
