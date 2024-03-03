using Core.Application.ServiceLifeTimes;
using MediatR;

namespace Core.Application.Entities.Commands;

public interface ICleanBaseCommand : IRequest, ICleanBaseIgnore
{
}
public interface ICleanBaseCommand<TResponse> : IRequest<TResponse>, ICleanBaseIgnore
{
}