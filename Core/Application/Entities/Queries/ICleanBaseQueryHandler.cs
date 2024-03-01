using MediatR;

namespace Core.Application.Entities.Queries;

public interface ICleanBaseQueryHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : ICleanBaseQuery
{
}

public interface ICleanBaseQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICleanBaseQuery<TResponse>
{
}
