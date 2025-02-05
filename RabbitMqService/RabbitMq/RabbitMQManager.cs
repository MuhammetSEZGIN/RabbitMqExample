using System;
using RabbitMqService.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
namespace RabbitMqService.RabbitMq;

public class RabbitMQManager : IRabbitMQManager
{

    private readonly RabbitMQOptions _rabbitMqOptions;
    private IConnection _connection;
    public RabbitMQManager(IOptions<RabbitMQOptions> rabbitMqOptions)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
    }
    
    public RabbitMQOptions RabbitMqOptions => _rabbitMqOptions;
  
    public async Task<IConnection> CreateConnectionAsync(){
        
        if (_connection == null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqOptions.HostName,
                    Port     = _rabbitMqOptions.Port,
                    UserName = _rabbitMqOptions.UserName,
                    Password = _rabbitMqOptions.Password
                };
                _connection = await factory.CreateConnectionAsync();
            }
            return _connection;
    }

}
