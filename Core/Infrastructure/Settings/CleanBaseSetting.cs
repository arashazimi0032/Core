using Core.Application.ServiceLifeTimes;
using Core.Domain.Enums;
using Core.Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Settings;

public abstract class CleanBaseSetting : ICleanBaseSingleton
{
    private readonly IConfiguration _configuration;

    protected CleanBaseSetting(IConfiguration configuration)
    {
        _configuration = configuration;
        GetConfigurationValues();
    }

    private void GetConfigurationValues()
    {
        var typeName = GetType().Name;
        var section = _configuration.GetSection(typeName);

        foreach (var property in GetType().GetProperties())
        {
            var configValue = section[property.Name];
            property.SetValue(this, ConvertType(configValue, property.PropertyType));
        }
    }

    private object? ConvertType(string? value, Type targetType)
    {
        if (value is null)
        {
            return null;
        }

        if (targetType.IsEnum)
        {
            if (Enum.TryParse(targetType, value, true, out object? result))
            {
                return result;
            }
            else
            {
                throw new CleanTemplateInternalException(
                    $"Invalid Enum value for type {targetType}: {value}", 
                    CleanBaseExceptionCode.ArgumentException,
                    new CleanTemplateInternalException(
                        $"Error in reading Settings from appsettings.json. Enum of type: {targetType} could not accept Value: {value}",
                        CleanBaseExceptionCode.CleanBaseSettingException));
            }
        }

        return Convert.ChangeType(value, targetType);
    }
}
