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
    public class UserDeactivatedDomainEventHandler : INotificationHandler<DomainEventNotification<UserDeactivated>>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public UserDeactivatedDomainEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(DomainEventNotification<UserDeactivated> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserDeactivatedIntegrationEvent(
                notification.DomainEvent.UserId.Value
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }

}
