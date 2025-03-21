using System.Diagnostics;
using ZHZ.EventBus;
using ZHZ.EventBus.Handler;

namespace IDentity.WebAPI.Event
{
    [EventName("IdentityService.User.Created")]
    public class UserCreatedEventHandler : JsonIntergrationEventHandler<UserCreatedEvent>
    {
       
        public override Task HandleJson(string eventName, UserCreatedEvent? eventData)
        {
            Console.WriteLine(eventData); 

            return Task.CompletedTask;
        }
    }
}
