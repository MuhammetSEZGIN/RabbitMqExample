using System;
using RabbitMqService.RabbitMq;
using RabbitMQ.Client;

namespace RabbitMqService.Interfaces;

public interface IRabbitMQManager
{
    public Task<IConnection> CreateConnectionAsync();
    public RabbitMQOptions RabbitMqOptions { get; } 

}
