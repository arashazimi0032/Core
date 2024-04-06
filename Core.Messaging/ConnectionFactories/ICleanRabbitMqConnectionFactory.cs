using RabbitMQ.Client;

namespace Core.Messaging.ConnectionFactories;

public interface ICleanRabbitMqConnectionFactory
{
    IConnection CreateConnection();
}
