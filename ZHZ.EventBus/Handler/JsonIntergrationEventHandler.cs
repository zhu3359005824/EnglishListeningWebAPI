using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZHZ.EventBus.Handler
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JsonIntergrationEventHandler<T> : IIntergrationEventHandler
    {
        public Task Handle(string eventName, string json)
        {
            T? eventData=JsonSerializer.Deserialize<T>(json);
            return HandleJson(eventName, eventData);
        }

        public abstract Task HandleJson(string eventName,T? eventData);
    }
}
