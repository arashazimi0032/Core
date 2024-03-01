using MediatR;

namespace Core.Application.Entities.Commands;

public interface ICleanBaseCommand : IRequest
{
}
public interface ICleanBaseCommand<TResponse> : IRequest<TResponse>
{
}
