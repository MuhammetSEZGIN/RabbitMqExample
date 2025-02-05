using System;
using System.Text;
using RabbitMqService.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqService.RabbitMq;

public class RabbitMQConsumer : IRabbitMQConsumer
{
    private readonly IRabbitMQManager _manager;
    private readonly ILogger<RabbitMQConsumer> _logger;

    public RabbitMQConsumer(IRabbitMQManager manager, ILogger<RabbitMQConsumer> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public async Task ConsumeAsync()
    {
        _logger.LogInformation("Starting to consume messages.");

        var connection = await _manager.CreateConnectionAsync();

        _logger.LogInformation("Connection created for consuming messages.");
        using var channel = await connection.CreateChannelAsync();
        _logger.LogInformation("Channel created for consuming messages.");

        var options = _manager.RabbitMqOptions;

        await channel.QueueDeclareAsync(
            queue: options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _logger.LogInformation("Queue declared: {QueueName}", options.QueueName);

        await channel.QueueBindAsync(
            queue: options.QueueName,
            exchange: options.ExchangeName,
            routingKey: options.RoutingKey);
        _logger.LogInformation("Queue bound to exchange: {ExchangeName} with routing key: {RoutingKey}", options.ExchangeName, options.RoutingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Message received: {Message}", message);
            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: options.QueueName,
            autoAck: true,
            consumer: consumer);
        _logger.LogInformation("Started consuming messages from queue: {QueueName}", options.QueueName);
        
    }
}