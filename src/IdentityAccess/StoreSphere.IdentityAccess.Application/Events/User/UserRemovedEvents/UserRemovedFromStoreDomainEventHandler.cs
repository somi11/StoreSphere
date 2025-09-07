using MediatR;
using StoreShpere.SharedKernel.Events;
using StoreShpere.SharedKernel.Notifications;
using StoreShpere.SharedKernel.UserIntegrationEvents.UserRemoved;
using StoreSphere.IdentityAccess.Domain.Events.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Events.User.UserRemovedEvents
{
    public class UserRemovedFromStoreDomainEventHandler : INotificationHandler<DomainEventNotification<UserRemovedFromStore>>
    {
        private readonly IIntegrationEventPublisher _publisher;
        public UserRemovedFromStoreDomainEventHandler(IIntegrationEventPublisher publisher) => _publisher = publisher;

        public async Task Handle(DomainEventNotification<UserRemovedFromStore> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserRemovedFromStoreIntegrationEvent(
                notification.DomainEvent.UserId.Value,
                notification.DomainEvent.StoreId.Value,
                notification.DomainEvent.RoleId.Value
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
