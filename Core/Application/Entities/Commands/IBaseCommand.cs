using MediatR;

namespace Core.Application.Entities.Commands;

public interface IBaseCommand : IRequest
{
}
public interface IBaseCommand<TResponse> : IRequest<TResponse>
{
}
