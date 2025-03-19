using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHZ.EventBus.Handler;

namespace ZHZ.EventBus.RabbitMQ
{/// <summary>
/// 在该类中
/// 1.完成交换机与队列的绑定 使用RabbitMQ的routing模式
/// 2.确定事件的监听者
/// 3.
/// </summary>
    public class RabbitMQEventBus : IEventBus
    {


        private readonly IOptionsSnapshot<IntergrationEventRabbitMQOption> _options;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly SubscriptionManager _subscriptionManager;

        private readonly IConnectionFactory _connectionFactory;

        private IChannel _channel;
        private IConnection _connection;



        public string ExchangeName { get; set; }
        public string HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public string QueueName { get; set; }

        public RabbitMQEventBus(IOptionsSnapshot<IntergrationEventRabbitMQOption> options, IServiceScopeFactory serviceScopeFactory, SubscriptionManager subscriptionManager, string queueName)
        {
            _options = options;
            _serviceScopeFactory = serviceScopeFactory;
            _subscriptionManager = subscriptionManager;
            HostName = _options.Value.HostName;
            Password = _options.Value.Password;
            ExchangeName = _options.Value.ExchangeName;
            UserName = _options.Value.UserName;
            QueueName = queueName;
            _subscriptionManager.OnEventRemoved += SubsManager_OnEventRemoved;
            _connectionFactory = new ConnectionFactory() { HostName = HostName };
            _channel = Start();
        }

        public IChannel Start()
        {
            _connection=_connectionFactory.CreateConnectionAsync().Result;
              
            
     var channel =_connection.CreateChannelAsync().Result;
            //匹配交换机
            channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct);
            //匹配队列
            channel.QueueDeclareAsync(QueueName,true,false,false,null);

            channel.CallbackExceptionAsync += (sender, ea) =>
            {
                Debug.Fail(ea.ToString());
                return null;
            };

            return channel;


        }
        

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = .CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: _exchangeName,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }

            using (var connection = _connectionFactory.CreateConnectionAsync().Result)
            using (var channel = connection.CreateChannelAsync().Result)
            {


            }


        }

        public void Pulish(string eventName, object? eventData)
        {
            
            throw new NotImplementedException();
           
        }

        public void Subscription(string eventName, Type handlerType)
        {
            throw new NotImplementedException();
        }

        public void UnSubscription(string eventName, Type handlerType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 对Handler进行检查
        /// </summary>
        /// <param name="handlerType"></param>
        /// <exception cref="ArgumentException"></exception>
        public void CheckHandlerType(Type handlerType) {
            if (!typeof(IIntergrationEventHandler).IsAssignableFrom(handlerType))
            {
                throw new ArgumentException($"{handlerType} 不能继承自 IIntergrationEventHandler");
            }
        }

        /// <summary>
        /// 将eventName作为routingKey进行交换机与队列的绑定
        /// </summary>
        /// <param name="eventName"></param>
        public void ChangeBindEvent(string eventName)
        {
           var containsKey=_subscriptionManager.HasSubscriptionsByEvent(eventName);

            if (!containsKey)
            {
                if (!_connection.IsOpen)
                {
                    _connection=_connectionFactory.CreateConnectionAsync().Result;
                }

//绑定交换机与队列   
                _channel.QueueBindAsync(QueueName, ExchangeName, eventName,null);


            }
        }

    }
}
