using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.EventBus.RabbitMQ;

namespace ZHZ.EventBus
{
    public interface IEventBus
    {
        






        void Pulish(string eventName, object? eventData);

        void Subscription(string eventName, Type handlerType);

        void UnSubscription(string eventName,Type handlerType);
    }
}
