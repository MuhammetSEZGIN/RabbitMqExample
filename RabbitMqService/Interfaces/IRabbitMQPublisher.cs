using System;

namespace RabbitMqService.Interfaces;

public interface IRabbitMQPublisher
{
    Task PublishAsync(string message);
}
