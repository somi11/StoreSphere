using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.Events
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IMediator _mediator;

        public IntegrationEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct = default)
            where TEvent : class
        {
            // If your integration events implement INotification, you can publish via MediatR
            if (integrationEvent is INotification notification)
            {
                await _mediator.Publish(notification, ct);
            }
            // Otherwise, you can add logic to publish to a message bus, etc.
        }
    }
}
