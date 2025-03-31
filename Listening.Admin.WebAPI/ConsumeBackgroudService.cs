
using ZHZ.EventBus.RabbitMQ;

namespace Listening.Admin.WebAPI
{
    public class ConsumeBackgroudService : BackgroundService
    {
        private readonly RabbitMQEventBus rabbitMQEventBus;

        public ConsumeBackgroudService(RabbitMQEventBus rabbitMQEventBus)
        {
            this.rabbitMQEventBus = rabbitMQEventBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }


    }
}
