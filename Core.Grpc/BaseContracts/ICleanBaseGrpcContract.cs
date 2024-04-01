using Core.Application.ServiceLifeTimes;
using System.ServiceModel;

namespace Core.Grpc.BaseContracts;

[ServiceContract]
public interface ICleanBaseGrpcContract : ICleanBaseIgnore
{
}
