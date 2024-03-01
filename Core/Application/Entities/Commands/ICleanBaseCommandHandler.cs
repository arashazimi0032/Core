using MediatR;

namespace Core.Application.Entities.Commands;

public interface ICleanBaseCommandHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : ICleanBaseCommand
{
}

public interface ICleanBaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICleanBaseCommand<TResponse>
{
}
