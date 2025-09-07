using MediatR;
using StoreSphere.IdentityAccess.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Infrastructure.Common
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(object domainEvent)
        {
            // MediatR expects INotification, so cast if possible
            if (domainEvent is INotification notification)
            {
                await _mediator.Publish(notification);
            }
            // Optionally, handle non-INotification events or throw
        }
    }
}
