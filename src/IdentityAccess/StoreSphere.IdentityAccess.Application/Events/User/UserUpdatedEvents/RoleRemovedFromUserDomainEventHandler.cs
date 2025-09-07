using MediatR;
using StoreShpere.SharedKernel.Events;
using StoreShpere.SharedKernel.Notifications;
using StoreShpere.SharedKernel.UserIntegrationEvents;
using StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated;
using StoreSphere.IdentityAccess.Domain.Events.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Events.User.UserUpdatedEvents
{
    public class RoleRemovedFromUserDomainEventHandler : INotificationHandler<DomainEventNotification<RoleRemovedFromUser>>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public RoleRemovedFromUserDomainEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(DomainEventNotification<RoleRemovedFromUser> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new RoleRemovedFromUserIntegrationEvent(
                notification.DomainEvent.UserId.Value,
                notification.DomainEvent.RoleId.Value
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
