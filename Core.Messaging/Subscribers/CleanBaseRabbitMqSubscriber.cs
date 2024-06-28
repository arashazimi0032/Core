using Core.Messaging.ConnectionFactories;
using Core.Messaging.Helper;
using Core.Messaging.Messages;
using Core.Messaging.Settings;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Core.Messaging.Subscribers;

public abstract class CleanBaseRabbitMqSubscriber : BackgroundService
{
}

public abstract class CleanBaseRabbitMqSubscriber<TMessage, TSetting> : CleanBaseRabbitMqSubscriber
    where TMessage : CleanBaseRabbitMqMessage
    where TSetting : CleanBaseRabbitMqSetting
{
    private readonly TSetting _settings;
    private readonly IModel _channel;

    protected CleanBaseRabbitMqSubscriber(ICleanRabbitMqConnectionFactory connectionFactory, TSetting settings)
    {
        try
        {
            _settings = settings;
            IConnection connection = connectionFactory.CreateConnection();

            _channel = connection.CreateModel();

            string queueName;
            if (!string.IsNullOrEmpty(_settings.ExchangeName))
            {
                _channel.ExchangeDeclare(_settings.ExchangeName, _settings.ExchangeType.ToString().ToLower(), false, false, null);

                if (_settings.RoutingKeies.Count != 0)
                {
                    foreach (string routingKey in _settings.RoutingKeies)
                    {
                        queueName = QueueDeclare();
                        _channel.QueueBind(queue: queueName, exchange: _settings.ExchangeName, routingKey: routingKey);
                    }
                }
                else
                {
                    queueName = QueueDeclare();
                    _channel.QueueBind(queue: queueName, exchange: _settings.ExchangeName, routingKey: string.Empty);
                }
            }
            else
            {
                QueueDeclare();
            }
        }
        catch (Exception ex)
        {
            RabbitMqExceptionHandling.HandleException(ex, this.GetType().Name, _settings, (TMessage)null);
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonConvert.DeserializeObject<TMessage>(content);

            try
            {
                await HandleAsync(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                RabbitMqExceptionHandling.HandleException(ex, this.GetType().Name, _settings, message);

                await Task.Delay(500);

                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };
        _channel.BasicConsume(_settings.QueueName, false, consumer);
        return Task.CompletedTask;
    }

    protected abstract Task HandleAsync(TMessage message);

    private string QueueDeclare()
    {
        string queueName;
        if (string.IsNullOrEmpty(_settings.QueueName))
        {
            queueName = _channel.QueueDeclare().QueueName;
        }
        else
        {
            _channel.QueueDeclare(_settings.QueueName, false, false, false, null);
            queueName = _settings.QueueName;
        }
        return queueName;
    }
}
