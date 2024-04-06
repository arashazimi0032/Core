using Core.Domain.Enums;
using Core.Messaging.ConnectionFactories;
using Core.Messaging.Exceptions;
using Core.Messaging.Messages;
using Core.Messaging.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Core.Messaging.Publishers;

public abstract class CleanBaseRabbitMqPublisher
{
}

public abstract class CleanBaseRabbitMqPublisher<TMessage, TSetting> : CleanBaseRabbitMqPublisher
    where TMessage : CleanBaseRabbitMqMessage
    where TSetting : CleanBaseRabbitMqSetting
{
    private IConnection _connection;
    private readonly ICleanRabbitMqConnectionFactory _connectionFactory;
    private readonly TSetting _settings;

    protected CleanBaseRabbitMqPublisher(ICleanRabbitMqConnectionFactory connectionFactory, TSetting settings)
    {
        _connectionFactory = connectionFactory;
        _settings = settings;
        _connection = _connectionFactory.CreateConnection();
    }

    public virtual void PublishAsync(TMessage message)
    {
        try
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.QueueDeclare(_settings.QueueName, false, false, false, null);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                if (!string.IsNullOrEmpty(_settings.ExchangeName))
                {
                    channel.ExchangeDeclare(_settings.ExchangeName, _settings.ExchangeType.ToString().ToLower(), false, false, null);

                    if (_settings.RoutingKeies.Count != 0)
                    {
                        foreach (string routingKey in _settings.RoutingKeies)
                        {
                            channel.BasicPublish(exchange: _settings.ExchangeName, routingKey: routingKey, null, body: body);
                        }
                    }
                    else
                    {
                        channel.BasicPublish(exchange: _settings.ExchangeName, routingKey: _settings.QueueName, null, body: body);
                    }
                }
                else
                {
                    channel.BasicPublish(exchange: _settings.ExchangeName, routingKey: _settings.QueueName, null, body: body);
                }
            }
        }
        catch (Exception ex)
        {
            throw new CleanTemplateMessagingInternalException(
                "CleanBaseRabbitMqPublisher Exception: Something went wrong. Could not publish message.",
                CleanBaseExceptionCode.CleanBaseRabbitMqPublisherException,
                new CleanTemplateMessagingInternalException(
                    ex.Message,
                    CleanBaseExceptionCode.Exception));
        }
    }

    private bool ConnectionExists()
    {
        if (_connection is not null)
        {
            return true;
        }
        _connection = _connectionFactory.CreateConnection();
        return true;
    }
}
