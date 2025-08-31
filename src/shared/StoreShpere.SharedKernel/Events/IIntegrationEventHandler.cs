using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.Events
{
    public interface IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
    {
        Task HandleAsync(TEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}
