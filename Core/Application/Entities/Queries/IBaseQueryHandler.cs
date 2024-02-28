using MediatR;

namespace Core.Application.Entities.Queries;

public interface IBaseQueryHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IBaseQuery
{
}

public interface IBaseQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IBaseQuery<TResponse>
{
}
