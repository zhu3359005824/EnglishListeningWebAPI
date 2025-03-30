using ZHZ.EventBus.RabbitMQ;
using ZHZ.JWT;

namespace GlobalConfigurations.配置类
{
    public class MyConfig
    {
        public JWTSettings JWTSettings { get; set; }
        public IntergrationEventRabbitMQOption IntergrationEventRabbitMQOption { get; set; }
        public MyRedisOption RedisOption { get; set; }
        public MyDbOption DbOption { get; set; }
    }
}
