using System.Text.Json;

namespace ZHZ.EventBus.Handler
{
    public abstract class DynamicIntegrationEventHandler : IIntergrationEventHandler
    {
        public Task Handle(string eventName, string eventData)
        {
            object c = JsonSerializer.Deserialize<object>(eventData)!;

            return HandleEncodingItem(eventName, c);
        }

        public abstract Task HandleEncodingItem(string eventName, object eventData);
    }
 
}
