using Core.Domain.Enums;
using Core.Grpc.Exceptions;
using Core.Grpc.Services;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Core.Grpc.Clients;

public abstract class CleanBaseGrpcClient : ICleanBaseGrpcClient
{
    private readonly ICleanGrpcClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    protected CleanBaseGrpcClient(ICleanGrpcClientFactory clientFactory, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;
        CreateClient();
    }

    private void CreateClient()
    {
        foreach (var property in GetType().GetProperties())
        {
            string ServerAddreass = _configuration.GetSection($"{GetType().Name}:ServerAddress").Value!;

            if (ServerAddreass is null)
            {
                throw new CleanTemplateGrpcInternalException(
                    "CleanBaseClient Exception: ServerAddress could not be null. Note that Your Client Name should be the same as the name of its section in the appsettings.json.",
                    CleanBaseExceptionCode.CleanBaseGrpcClientException,
                    new CleanTemplateGrpcInternalException(
                        "CleanBaseClient Exception: ServerAddress could not be null. Note that Your Client Name should be the same as the name of its section in the appsettings.json.",
                        CleanBaseExceptionCode.ArgumentNullException));
            }

            Type contractType = property.PropertyType;

            MethodInfo createClientMethod = _clientFactory.GetType().GetMethod("CreateGrpcClient")!;

            MethodInfo genericMethod = createClientMethod.MakeGenericMethod(contractType);

            object[] parameters = [ServerAddreass];

            object client = genericMethod.Invoke(_clientFactory, parameters)!;

            property.SetValue(this, client);
        }
    }
}
