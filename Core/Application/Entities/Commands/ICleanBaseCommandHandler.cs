using Core.Application.ServiceLifeTimes;
using MediatR;

namespace Core.Application.Entities.Commands;

public interface ICleanBaseCommandHandler<TRequest> : IRequestHandler<TRequest>, ICleanBaseIgnore
    where TRequest : ICleanBaseCommand
{
}

public interface ICleanBaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, ICleanBaseIgnore
    where TRequest : ICleanBaseCommand<TResponse>
{
}
