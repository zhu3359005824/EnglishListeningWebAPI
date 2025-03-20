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
        





        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        void Pulish(string eventName, object? eventData);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        void Subscribe(string eventName, Type handlerType);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        void UnSubscribe(string eventName,Type handlerType);
    }
}
