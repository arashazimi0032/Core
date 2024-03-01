using Core.Application.ServiceLifeTimes;
using Core.Domain.BaseEvents;
using MediatR;

namespace Core.Application.Entities.Events;

public interface ICleanBaseDomainEventHandler<TNotification> : INotificationHandler<TNotification>, ICleanBaseIgnore
    where TNotification : ICleanBaseDomainEvent
{
}
