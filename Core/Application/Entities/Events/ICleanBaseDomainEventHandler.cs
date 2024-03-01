using Core.Domain.BaseEvents;
using MediatR;

namespace Core.Application.Entities.Events;

public interface ICleanBaseDomainEventHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : ICleanBaseDomainEvent
{
}
