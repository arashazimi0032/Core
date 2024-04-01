using Core.Domain.Enums;
using Core.Grpc.BaseContracts;
using Core.Grpc.Exceptions;
using Grpc.Net.Client;
using ServiceModel.Grpc.Client;

namespace Core.Grpc.Services;

public class CleanGrpcClientFactory : ICleanGrpcClientFactory
{
    public TContract CreateGrpcClient<TContract>(string serverAddress)
        where TContract : class, ICleanBaseGrpcContract
    {
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var channel = GrpcChannel.ForAddress(serverAddress, new GrpcChannelOptions() { HttpHandler = httpHandler });

        var clientFactory = new ClientFactory();

        var client = clientFactory.CreateClient<TContract>(channel);

        if (client is null)
        {
            throw new CleanTemplateGrpcInternalException(
                $"CleanGrpcClientFactory Exception: Could Not create a Client of type {typeof(TContract).Name}.", 
                CleanBaseExceptionCode.CleanGrpcClientFactoryException,
                new CleanTemplateGrpcInternalException(
                    $"CleanGrpcClientFactory Exception: Could Not create a Client of type {typeof(TContract).Name}.",
                    CleanBaseExceptionCode.ArgumentNullException));
        }

        return client;
    }
}
