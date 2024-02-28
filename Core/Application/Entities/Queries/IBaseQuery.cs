using MediatR;

namespace Core.Application.Entities.Queries;

public interface IBaseQuery : IRequest
{
}

public interface IBaseQuery<TResponse> : IRequest<TResponse>
{
}
