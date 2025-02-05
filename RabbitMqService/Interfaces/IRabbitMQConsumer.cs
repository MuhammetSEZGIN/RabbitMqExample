using System;

namespace RabbitMqService.Interfaces;

public interface IRabbitMQConsumer
{
     Task ConsumeAsync();
}
