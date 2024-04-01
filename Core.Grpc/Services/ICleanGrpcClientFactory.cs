using Core.Grpc.BaseContracts;

namespace Core.Grpc.Services;

public interface ICleanGrpcClientFactory
{
    TContract CreateGrpcClient<TContract>(string serverAddress)
        where TContract : class, ICleanBaseGrpcContract;
}
