using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreShpere.SharedKernel.Events
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync(object integrationEvent, CancellationToken ct = default);
        Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken ct = default)
            where TEvent : class;
    }
}
