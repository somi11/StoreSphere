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
    public class UserEmailChangedDomainEventHandler : INotificationHandler<DomainEventNotification<UserEmailChanged>>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public UserEmailChangedDomainEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(DomainEventNotification<UserEmailChanged> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserEmailChangedIntegrationEvent(
                notification.DomainEvent.UserId.Value,
                notification.DomainEvent.NewEmail.Value
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }

   
}
