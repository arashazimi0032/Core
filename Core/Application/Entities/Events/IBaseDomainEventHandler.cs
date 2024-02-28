using Core.Domain.BaseEvents;
using MediatR;

namespace Core.Application.Entities.Events;

public interface IBaseDomainEventHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : IBaseDomainEvent
{
}
