using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Reflection;
using ZHZ.EventBus.Handler;
using ZHZ.EventBus.RabbitMQ;

namespace ZHZ.EventBus
{
    public static class EventBusExtension
    {
        /// <summary>
        /// 该方法获得所有实现了IIntergrationEventHandler的类,并且将参数传给另一个AddEventBus
        /// </summary>
        /// <param name="services"></param>
        /// <param name="queueName"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, string queueName, IEnumerable<Assembly> assemblies)
        {
            var eventHandlers = new List<Type>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    // 使用GetExportedTypes避免加载内部类型，添加异常处理
                    var types = assembly.GetExportedTypes()
                        .Where(t =>
                            !t.IsAbstract &&
                            !t.IsInterface &&
                            typeof(IIntergrationEventHandler).IsAssignableFrom(t)
                        ).ToList();

                    eventHandlers.AddRange(types);
                }
                catch (ReflectionTypeLoadException ex)  // 处理类型加载失败的情况
                {
                    var loadedTypes = ex.Types.Where(t => t != null);
                    var validTypes = loadedTypes.Where(t =>
                        !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(IIntergrationEventHandler).IsAssignableFrom(t)
                    );
                    eventHandlers.AddRange(validTypes);
                }
            }

            return AddEventBus(services, queueName, eventHandlers);
        }

        /// <summary>
        /// 该方法将处理器的事件保存到事件管理器SubscriptionManager中
        /// </summary>
        /// <param name="services"></param>
        /// <param name="queueName"></param>
        /// <param name="eventHandlers"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static IServiceCollection AddEventBus(IServiceCollection services, string queueName, IEnumerable<Type> eventHandlers)
        {
            foreach (var type in eventHandlers)
            {
                services.AddScoped(type, type);
            }

            services.AddSingleton<IEventBus>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<IntergrationEventRabbitMQOption>>().Value;

                var factory = new ConnectionFactory()
                {
                    HostName = options.HostName,
                    ConsumerDispatchConcurrency = 2
                };
                if (options.UserName != null)
                {
                    factory.UserName = options.UserName;
                }
                if (options.Password != null)
                {
                    factory.Password = options.Password;
                }


                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                var eventBus = new RabbitMQEventBus(factory, serviceScopeFactory, queueName, options.ExchangeName);

                foreach (var type in eventHandlers)
                {
                    var eventNameAttributes = type.GetCustomAttributes<EventNameAttribute>();
                    if (eventNameAttributes.Any() == false)
                    {
                        throw new Exception($"应该至少有一个EventNameAttribute在处理器{type}上");
                    }

                    foreach (var item in eventNameAttributes)
                    {
                        eventBus.Subscribe(item.Name, type);
                    }
                }
                return eventBus;


            });

            return services;
        }







        public static IServiceCollection AddEventBus(this IServiceCollection services, string queueName,
           params Assembly[] assemblies)
        {
            return AddEventBus(services, queueName, assemblies.ToList());
        }
    }
}
