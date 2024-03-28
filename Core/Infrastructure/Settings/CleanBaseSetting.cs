using Core.Application.ServiceLifeTimes;
using Core.Domain.Enums;
using Core.Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Settings;

public abstract class CleanBaseSetting : ICleanBaseSingleton
{
    protected CleanBaseSetting(IConfiguration configuration)
    {
		try
		{
			configuration.GetSection(GetType().Name).Bind(this);
		}
		catch (Exception ex)
		{
			throw new CleanTemplateInternalException(
				"An Error has occured in CleanBaseSetting", 
				CleanBaseExceptionCode.CleanBaseSettingException, 
				ex);
		}
    }
}
