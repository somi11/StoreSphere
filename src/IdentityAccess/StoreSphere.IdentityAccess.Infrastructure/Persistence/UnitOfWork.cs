using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly DomainDbContext _domainDbContext;
        private readonly IDomainEventDispatcher _dispatcher;

        public UnitOfWork(AppIdentityDbContext identityDbContext, DomainDbContext dbContext, IDomainEventDispatcher dispatcher)
        {
            _identityDbContext = identityDbContext;
            _domainDbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {

            using var transaction = await _domainDbContext.Database.BeginTransactionAsync(ct);

            try
            {
                // Save domain
                var domainResult = await _domainDbContext.SaveChangesAsync(ct);

                // Save identity
                await _identityDbContext.SaveChangesAsync(ct);

                // Dispatch domain events (AFTER persistence)
                var aggregates = _domainDbContext.ChangeTracker
                    .Entries()
                    .Select(e => e.Entity)
                    .OfType<AggregateRoot<UserId>>()
                    .Where(a => a.DomainEvents.Any())
                    .ToList();

                foreach (var aggregate in aggregates)
                {
                    foreach (var domainEvent in aggregate.DomainEvents)
                    {
                        await _dispatcher.DispatchAsync(domainEvent);
                    }
                    aggregate.ClearDomainEvents();
                }

                // Commit
                await transaction.CommitAsync(ct);

                return domainResult;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
