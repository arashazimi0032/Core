using Core.Application.ServiceLifeTimes;
using MediatR;

namespace Core.Application.Entities.Queries;

public interface ICleanBaseQueryHandler<TRequest> : IRequestHandler<TRequest>, ICleanBaseIgnore
    where TRequest : ICleanBaseQuery
{
}

public interface ICleanBaseQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, ICleanBaseIgnore
    where TRequest : ICleanBaseQuery<TResponse>
{
}
