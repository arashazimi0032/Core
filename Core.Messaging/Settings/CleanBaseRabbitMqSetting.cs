using Core.Infrastructure.Settings;
using Core.Messaging.Enums;
using Microsoft.Extensions.Configuration;

namespace Core.Messaging.Settings;

public abstract class CleanBaseRabbitMqSetting : CleanBaseSetting
{
    protected CleanBaseRabbitMqSetting(IConfiguration configuration) : base(configuration)
    {
    }

    public string QueueName { get; set; } = string.Empty;
    public string ExchangeName { get; set; } = string.Empty;
    public List<string> RoutingKeies { get; set; } = new List<string>();
    public CleanExchangeType ExchangeType { get; set; } = CleanExchangeType.Fanout;
}
