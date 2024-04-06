using Core.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;

namespace Core.Messaging.Settings;

public sealed class CleanRabbitMqHostSetting : CleanBaseSetting
{
    public CleanRabbitMqHostSetting(IConfiguration configuration) : base(configuration)
    {
    }

    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
