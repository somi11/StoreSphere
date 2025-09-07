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
    public class UserTypeChangedDomainEventHandler : INotificationHandler<DomainEventNotification<UserTypeChanged>>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public UserTypeChangedDomainEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(DomainEventNotification<UserTypeChanged> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserTypeChangedIntegrationEvent(
                notification.DomainEvent.UserId.Value,
                notification.DomainEvent.UserType.ToString()
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }

    }

}
