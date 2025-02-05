
using RabbitMqService.Interfaces;

namespace RabbitMqService.Services
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IRabbitMQConsumer _consumer;
        private readonly ILogger<RabbitMQConsumerService> _logger;

        public RabbitMQConsumerService(IRabbitMQConsumer consumer, ILogger<RabbitMQConsumerService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting RabbitMQ Consumer Service");
            await _consumer.ConsumeAsync();
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
