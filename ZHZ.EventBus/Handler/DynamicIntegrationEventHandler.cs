using System.Text.Json;

namespace ZHZ.EventBus.Handler
{
    public abstract class DynamicIntegrationEventHandler : IIntergrationEventHandler
    {
        public Task Handle(string eventName, string eventData)
        {
            dynamic c = JsonSerializer.Deserialize<dynamic>(eventData);

            return HandleDynamic(eventName, c);
        }

        public abstract Task HandleDynamic(string eventName, dynamic eventData);
    }
}
