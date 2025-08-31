using MediatR;
using StoreShpere.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.Notifications
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
    {
        public TDomainEvent DomainEvent { get; }
        public DomainEventNotification(TDomainEvent domainEvent) => DomainEvent = domainEvent;
    }
}
