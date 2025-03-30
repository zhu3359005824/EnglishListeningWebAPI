using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using ZHZ.EventBus.Handler;

namespace ZHZ.EventBus.RabbitMQ
{/// <summary>
/// 事件总线核心类
/// 在该类中
/// 1.完成交换机与队列的绑定 使用RabbitMQ的routing模式
/// 2.确定事件的监听者
/// 
/// </summary>
    public class RabbitMQEventBus : IEventBus
    {



        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly SubscriptionManager _subscriptionManager;

        private readonly IConnectionFactory _connectionFactory;

        private IChannel _channel;

        private IConnection _connection;

        private readonly IServiceScope _serviceScope;



        public string ExchangeName { get; set; }


        public string QueueName { get; set; }

        public RabbitMQEventBus(IConnectionFactory connectionFactory, IServiceScopeFactory serviceScopeFactory, string queueName, string exchangeName)
        {
            ExchangeName = exchangeName;
            _serviceScopeFactory = serviceScopeFactory;

            _serviceScope = _serviceScopeFactory.CreateScope();
            _subscriptionManager = new SubscriptionManager();

            QueueName = queueName;
            _subscriptionManager.OnEventRemoved += SubsManager_OnEventRemoved;
            _connectionFactory = connectionFactory;
            _channel = Start();

        }

        public IChannel Start()
        {
            _connection = _connectionFactory.CreateConnectionAsync().Result;
            if (_connection == null)
                throw new InvalidOperationException("Failed to create RabbitMQ connection.");

            var channel = _connection.CreateChannelAsync().Result;
            if (channel == null)
                throw new InvalidOperationException("Failed to create RabbitMQ channel.");

            // 匹配交换机
            channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct);
            // 匹配队列
            channel.QueueDeclareAsync(QueueName, true, false, false, null);

            channel.CallbackExceptionAsync += (sender, ea) =>
            {
                Debug.Fail(ea.ToString());
                return null;
            };

            return channel;
        }

        /// <summary>
        /// 当事件管理器中有事件移除时触发的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            if (_connection.IsOpen)
            {
                using (var channel = _connection.CreateChannelAsync().Result)
                {
                    channel.QueueUnbindAsync(QueueName, ExchangeName, eventName);
                }
                if (_subscriptionManager.IsEmpty)
                {
                    QueueName = string.Empty;
                    _channel.CloseAsync();
                }


            }
        }

        public async void Publish(string eventName, object? eventData)
        {

            if (_connection.IsOpen)
            {
                using (var channel = _connection.CreateChannelAsync().Result)
                {
                    await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct);

                    byte[] body;

                    if (eventData == null) body = new byte[0];
                    else
                    {
                        JsonSerializerOptions options = new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                        };
                        body = JsonSerializer.SerializeToUtf8Bytes(eventData, eventData.GetType(), options);
                    }


                    var property = new BasicProperties()
                    {
                        DeliveryMode = DeliveryModes.Persistent

                    };


                    await channel.BasicPublishAsync(ExchangeName, eventName, true, property, body);

                }

            }

        }

        public void Subscribe(string eventName, Type handlerType)
        {
            CheckHandlerType(handlerType);
            ChangeBindEvent(eventName);
            _subscriptionManager.AddSubscription(eventName, handlerType);

            StartBasicConsume();
        }

        //取消订阅
        public void UnSubscribe(string eventName, Type handlerType)
        {
            CheckHandlerType(handlerType);

            _subscriptionManager.RemoveSubscription(eventName, handlerType);
        }

        /// <summary>
        /// 对Handler进行检查
        /// </summary>
        /// <param name="handlerType"></param>
        /// <exception cref="ArgumentException"></exception>
        public void CheckHandlerType(Type handlerType)
        {
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
            var containsKey = _subscriptionManager.HasSubscriptionsByEvent(eventName);

            if (!containsKey)
            {
                if (!_connection.IsOpen)
                {
                    _connection = _connectionFactory.CreateConnectionAsync().Result;
                }

                //绑定交换机与队列   
                _channel.QueueBindAsync(QueueName, ExchangeName, eventName, null);


            }
        }




        public void Dispose()
        {
            if (_connection != null)
            {
                _channel.Dispose();
            }

            _subscriptionManager.Clear();

            _connection.Dispose();

            _serviceScope.Dispose();

        }

        /// <summary>
        /// 开始消费
        /// </summary>
        private async void StartBasicConsume()
        {
            if (_channel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += Consumer_Received;

                await _channel.BasicConsumeAsync(QueueName, false, consumer);
            }
        }

        /// <summary>
        /// 消费者接受事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;

            //框架要求所有的消息都是字符串的json
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);


            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }




        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subscriptionManager.HasSubscriptionsByEvent(eventName))
            {
                var subscriptionsHandlerType = _subscriptionManager.GetHandlerByEvent(eventName);

                foreach (var handlerType in subscriptionsHandlerType)
                {
                    //各自在不同的Scope中，避免DbContext等的共享造成如下问题：
                    //The instance of entity type cannot be tracked because another instance
                    using var scope = _serviceScope.ServiceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetService(handlerType) as IIntergrationEventHandler;

                    if (handler == null)
                    {
                        throw new ApplicationException($"无法创建{handlerType}的服务");
                    }

                    await handler.Handle(eventName, message);
                }
            }
            else
            {
                string entryAsm = Assembly.GetEntryAssembly().GetName().Name;
                Debug.WriteLine($"找不到可以处理eventName={eventName}的处理程序，entryAsm:{entryAsm}");
            }
        }
    }
}