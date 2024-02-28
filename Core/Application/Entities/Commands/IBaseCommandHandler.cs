using MediatR;

namespace Core.Application.Entities.Commands;

public interface IBaseCommandHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IBaseCommand
{
}

public interface IBaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IBaseCommand<TResponse>
{
}
