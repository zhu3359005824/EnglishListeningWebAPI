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
        private IChannel _channel_consume;

        private IConnection _connection;

        private readonly IServiceScope _serviceScope;

        public bool IsConsume=false;



        public string ExchangeName { get; set; }


        public string QueueName { get; set; }

        public RabbitMQEventBus(IConnectionFactory connectionFactory, IServiceScopeFactory serviceScopeFactory, string queueName, string exchangeName)
        {
            ExchangeName = exchangeName;
            _serviceScopeFactory = serviceScopeFactory;
            _subscriptionManager = new SubscriptionManager();
            QueueName = queueName;
            _subscriptionManager.OnEventRemoved += SubsManager_OnEventRemoved;
            _connectionFactory = connectionFactory;
            _channel = Start();
           // _channel_consume = ConsumeChannel();
            _channel.BasicQosAsync(0,prefetchCount:1,true);
           // _channel_consume.BasicQosAsync(0, prefetchCount: 1, true);

        }

        public IChannel ConsumeChannel()
        {
            var channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
            channel.ContinuationTimeout =TimeSpan.FromMinutes(1);
            channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic).GetAwaiter().GetResult();
            channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null)
                .GetAwaiter().GetResult();
            return channel;
        }

        public IChannel Start()
        {
            try
            {
                _connection = _connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
               
                var channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
                channel.ContinuationTimeout = TimeSpan.FromMinutes(1);
                channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic).GetAwaiter().GetResult();
                channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null)
                    .GetAwaiter().GetResult();
                return channel;
            }
            catch (Exception ex)
            {
                Debug.Fail($"RabbitMQ初始化失败: {ex}");
                throw;
            }
        }




        /// <summary>
        /// 生产者代码
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public async void Publish(string eventName, object? eventData)
        {
            if (!_connection.IsOpen)
            {
                Debug.Fail("连接未打开，无法发布消息。");
                return;
            }
            try
            {
                
                    var body = eventData == null
                        ? new byte[0]
                        : JsonSerializer.SerializeToUtf8Bytes(eventData);

                    var properties = new BasicProperties { DeliveryMode = DeliveryModes.Persistent };
                  await  _channel.BasicPublishAsync(
                        exchange: ExchangeName,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body
                    );
                
                    Console.WriteLine($"已发布事件{eventName}到交换机{ExchangeName}");
                
            }
            catch (Exception ex)
            {
                Debug.Fail($"发布消息失败: {ex}");
            }
        }

        /// <summary>
        /// 订阅服务
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerType"></param>
        public void Subscribe(string eventName, Type handlerType)
        {
            CheckHandlerType(handlerType);
            ChangeBindEvent(eventName);
            _subscriptionManager.AddSubscription(eventName, handlerType);
            StartBasicConsume();
        }

        /// <summary>
        /// 开始消费
        /// </summary>
        public async void StartBasicConsume()
        {
            if (_channel != null && _channel.IsOpen)
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += Consumer_Received;

                try
                {
                    await _channel.BasicConsumeAsync(
                        queue: QueueName,
                        autoAck: false, // 手动确认消息
                        consumer: consumer
                    );
                    Console.WriteLine($"已启动消费者监听队列: {QueueName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"启动消费者失败: {ex}");
                }
            }
            else
            {
                Console.WriteLine("通道未初始化或已关闭，无法启动消费者。");
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
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetService(handlerType) as IIntergrationEventHandler;
                        if (handler == null)
                        {
                            Debug.Fail($"无法创建{handlerType}的实例");
                            continue;
                        }

                        try
                        {
                            await handler.Handle(eventName, message);
                        }
                        catch (Exception ex)
                        {
                            Debug.Fail($"处理事件{eventName}时出错: {ex}");
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine($"没有找到处理事件{eventName}的订阅者。");
            }
        }

        /// <summary>
        /// 将eventName作为routingKey进行交换机与队列的绑定
        /// </summary>
        /// <param name="eventName"></param>
        private void ChangeBindEvent(string eventName)
        {
            if (!_subscriptionManager.HasSubscriptionsByEvent(eventName))
            {
                if (!_connection.IsOpen)
                {
                    // 尝试重新连接
                    _connection = _connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
                }

                // 绑定队列到交换机，使用eventName作为路由键
                _channel.QueueBindAsync(
                    queue: QueueName,
                    exchange: ExchangeName,
                    routingKey: eventName
                ).GetAwaiter().GetResult();
            }
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
    }
}