using MediatR;
using StoreShpere.SharedKernel.Events;
using StoreShpere.SharedKernel.Notifications;
using StoreShpere.SharedKernel.UserIntegrationEvents.UserRemoved;
using StoreShpere.SharedKernel.UserIntegrationEvents.UserUpdated;
using StoreSphere.IdentityAccess.Domain.Events.Store;
using StoreSphere.IdentityAccess.Domain.Events.Tenant;
using StoreSphere.IdentityAccess.Domain.Events.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Events.User.UserRemovedEvents
{
    public class UserRemovedFromTenantDomainEventHandler : INotificationHandler<DomainEventNotification<UserRemovedFromTenant>>
    {
        private readonly IIntegrationEventPublisher _publisher;
        public UserRemovedFromTenantDomainEventHandler(IIntegrationEventPublisher publisher) => _publisher = publisher;

        public async Task Handle(DomainEventNotification<UserRemovedFromTenant> notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserRemovedFromTenantIntegrationEvent(
                notification.DomainEvent.UserId.Value,
                notification.DomainEvent.TenantId.Value
            );
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }

}
