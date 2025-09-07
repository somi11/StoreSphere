using MediatR;
using StoreShpere.SharedKernel.Events;
using StoreShpere.SharedKernel.Notifications;
using StoreShpere.SharedKernel.UserIntegrationEvents.UserRegistered;
using StoreSphere.IdentityAccess.Domain.Events.User;


namespace StoreSphere.IdentityAccess.Application.Events.User.UserRegisteredEvents
{
    public class UserRegisteredDomainEventHandler
        : INotificationHandler<DomainEventNotification<UserRegistered>>
    {
        private readonly IIntegrationEventPublisher _publisher;

        public UserRegisteredDomainEventHandler(IIntegrationEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(DomainEventNotification<UserRegistered> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            // Map domain event → integration event
            var integrationEvent = new UserRegisteredIntegrationEvent(
                 domainEvent.UserId.Value,
                 domainEvent.Email.Value,
                 domainEvent.UserType.ToString(),
                 domainEvent.TenantId?.Value,
                 domainEvent.IdentityId // Pass IdentityId if available
             );

            // Publish globally (other modules can subscribe)
            await _publisher.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
