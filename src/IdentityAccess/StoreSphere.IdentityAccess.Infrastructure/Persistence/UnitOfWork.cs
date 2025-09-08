using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Domain.common;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DomainDbContext _dbContext;
        private readonly IDomainEventDispatcher _dispatcher;

        public UnitOfWork(DomainDbContext dbContext, IDomainEventDispatcher dispatcher)
        {
            _dbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            // 1. Find all aggregates with domain events
            //var aggregates = _dbContext.ChangeTracker
            //    .Entries()
            //    .Where(e => e.Entity is AggregateRoot<object>)
            //    .Select(e => e.Entity as AggregateRoot<object>)
            //    .Where(a => a != null && a.DomainEvents.Any())
            //    .ToList();
            var aggregates = _dbContext.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<AggregateRoot<UserId>>()   // repeat for each aggregate type
            .Where(a => a.DomainEvents.Any())
            .ToList();
            // 2. Dispatch each domain event
            foreach (var aggregate in aggregates)
            {
                foreach (var domainEvent in aggregate.DomainEvents)
                {
                    await _dispatcher.DispatchAsync(domainEvent);
                }
                // 3. Clear domain events after dispatch
                aggregate.ClearDomainEvents();
            }

            // 4. Save changes to the database
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
