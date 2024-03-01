using Core.Application.ServiceLifeTimes;
using MediatR;

namespace Core.Application.Entities.Queries;

public interface ICleanBaseQuery : IRequest, ICleanBaseIgnore
{
}

public interface ICleanBaseQuery<TResponse> : IRequest<TResponse>, ICleanBaseIgnore
{
}
