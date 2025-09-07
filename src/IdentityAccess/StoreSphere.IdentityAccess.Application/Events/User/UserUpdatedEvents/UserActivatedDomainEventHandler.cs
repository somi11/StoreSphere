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
    public class UserActivatedDomainEventHandler : INotificationHandler<DomainEventNotification<UserActivated>>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public UserActivatedDomainEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(DomainEventNotification<UserActivated> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserActivatedIntegrationEvent(
                notification.DomainEvent.UserId.Value
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
