using Microsoft.AspNetCore.Builder;

namespace ZHZ.EventBus
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
        {
            // 获得IEventBus一次，就会立即加载IEventBus，这样扫描所有的EventHandler，保证消息及时消费


            object? eventBus = app.ApplicationServices.GetService(typeof(IEventBus));

            if (eventBus == null)
            {
                throw new ApplicationException("找不到IEventBus实例");
            }
            return app;
        }
    }
}
