using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHZ.EventBus
{/// <summary>
/// 订阅事件管理器,
///
/// </summary>
    public class SubscriptionManager
    {
        /// <summary>
        /// 事件,监听该事件者Handler
        /// </summary>
        private readonly Dictionary<string,List<Type>> _handlers = new Dictionary<string,List<Type>>();


        public event EventHandler<string> OnEventRemoved;



        public bool IsEmpty=> _handlers.Keys.Any();


        public void Clear()=>_handlers.Clear();


        /// <summary>
        /// 将事件添加到Handler中
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandlerType"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddSubscription(string eventName,Type eventHandlerType)
        {
            if (!HasSubscriptionsByEvent(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }


            //如果已经注册过，则报错
            if (_handlers[eventName].Contains(eventHandlerType))
            {
                throw new ArgumentException($"Handler Type {eventHandlerType} already registered for '{eventName}'", nameof(eventHandlerType));
            }

            _handlers[eventName].Add(eventHandlerType);
        }

        public void RemoveSubscription(string eventName, Type handlerType)
        {
            _handlers[eventName].Remove(handlerType);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
            }
            OnEventRemoved?.Invoke(this,eventName);
        }
        /// <summary>
        /// 获取eventName的所有Handler
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetHandlerByEvent(string eventName) => _handlers[eventName];

        /// <summary>
        /// 是否含有eventName事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public bool HasSubscriptionsByEvent(string eventName)=>_handlers.ContainsKey(eventName);

    }
}
