using Core.Application.ServiceLifeTimes;
using MediatR;

namespace Core.Domain.BaseEvents;

public interface ICleanBaseDomainEvent : INotification, ICleanBaseIgnore
{
}