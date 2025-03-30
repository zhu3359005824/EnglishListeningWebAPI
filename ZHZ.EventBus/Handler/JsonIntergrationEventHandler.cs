using System.Text.Json;

namespace ZHZ.EventBus.Handler
{
    /// <summary>
    /// 将事件数据转换成类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JsonIntergrationEventHandler<T> : IIntergrationEventHandler
    {
        public Task Handle(string eventName, string json)
        {
            T? eventData = JsonSerializer.Deserialize<T>(json);
            return HandleJson(eventName, eventData);
        }

        public abstract Task HandleJson(string eventName, T? eventData);
    }
}
