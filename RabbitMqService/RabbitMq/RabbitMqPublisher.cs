using System.Text;
using RabbitMQ.Client;
using RabbitMqService.Interfaces;

namespace RabbitMqService.RabbitMq
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IRabbitMQManager _manager;
        private readonly ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(IRabbitMQManager manager, ILogger<RabbitMQPublisher> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public async Task PublishAsync(string message)
        {
            _logger.LogInformation("Starting to publish message.");

            var connection = await _manager.CreateConnectionAsync();
            _logger.LogInformation("Connection created.");

            using var channel = await connection.CreateChannelAsync();
            _logger.LogInformation("Channel created.");

            await channel.ExchangeDeclareAsync(
                exchange: _manager.RabbitMqOptions.ExchangeName,
                type: ExchangeType.Fanout);
            _logger.LogInformation("Exchange declared: {ExchangeName}", _manager.RabbitMqOptions.ExchangeName);

            var body = Encoding.UTF8.GetBytes(message);
            var props = new BasicProperties();
            props.Persistent = true;

            await channel.BasicPublishAsync(
                exchange: _manager.RabbitMqOptions.ExchangeName,
                routingKey: _manager.RabbitMqOptions.RoutingKey,
                mandatory: false,
                basicProperties: props,
                body: body);
            _logger.LogInformation("Message published to exchange: {ExchangeName} with routing key: {RoutingKey}", _manager.RabbitMqOptions.ExchangeName, _manager.RabbitMqOptions.RoutingKey);
        }
    }
}
