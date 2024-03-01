using MediatR;

namespace Core.Application.Entities.Queries;

public interface ICleanBaseQuery : IRequest
{
}

public interface ICleanBaseQuery<TResponse> : IRequest<TResponse>
{
}
