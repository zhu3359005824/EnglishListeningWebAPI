﻿namespace ZHZ.EventBus.RabbitMQ
{/// <summary>
/// 集成事件RabbitMQ配置类
/// </summary>
    public class IntergrationEventRabbitMQOption
    {
        public string ExchangeName { get; set; }

        public string HostName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
