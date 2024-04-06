using Core.Domain.Enums;
using Core.Messaging.Exceptions;
using Core.Messaging.Settings;
using RabbitMQ.Client;

namespace Core.Messaging.ConnectionFactories;

public class CleanRabbitMqConnectionFactory : ICleanRabbitMqConnectionFactory
{
    private readonly CleanRabbitMqHostSetting _hostSetting;

    public CleanRabbitMqConnectionFactory(CleanRabbitMqHostSetting hostSetting)
    {
        _hostSetting = hostSetting;
    }

    public IConnection CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostSetting.HostName,
                Password = _hostSetting.Password,
                UserName = _hostSetting.UserName,
            };

            return factory.CreateConnection();
        }
        catch (Exception ex)
        {
            throw new CleanTemplateMessagingInternalException(
                "CleanRabbitMqConnectionFactory Exception: Could not create RabbitMQ Connection!",
                CleanBaseExceptionCode.CleanRabbitMqConnectionFactoryException,
                new CleanTemplateMessagingInternalException(
                    ex.Message,
                    CleanBaseExceptionCode.Exception));
        }
    }
}
